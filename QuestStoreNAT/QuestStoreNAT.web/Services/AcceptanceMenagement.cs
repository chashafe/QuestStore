using System;
using Microsoft.AspNetCore.Mvc;
using QuestStoreNAT.web.DatabaseLayer;
using QuestStoreNAT.web.Models;
using QuestStoreNAT.web.Services;
namespace QuestStoreNAT.web.Services
{
    public class AcceptanceMenagement
    {
        private StudentAcceptanceDAO _studentAcceptanceDAO { get; set; }
        private GroupTransactionDAO _groupTrasanctionDAO {get; set;}

        public AcceptanceMenagement()
        {
            _studentAcceptanceDAO = new StudentAcceptanceDAO();
            _groupTrasanctionDAO = new GroupTransactionDAO();
        }

        public void StudentAcceptance(string answear, int groupID,int artifactID, int artifactCost, int studentID)
        {
            switch(answear)
            {
                case "No, I don't agree":
                    _studentAcceptanceDAO.DeleteAllTransactionForGroup(groupID);
                    _groupTrasanctionDAO.DeleteAllTransactionForGroup(groupID);
                    break;
                case "Accept":
                    if(CheckAmountOfAcceptance(groupID) == true)
                    {
                        CreateAndAddNewRecordGroupArtifact(groupID, artifactID);
                        UpdateValueStudentWallet(groupID, artifactID);
                        new ArtifactManagement().UpdateGroupWallet(groupID, artifactCost);
                        _groupTrasanctionDAO.DeleteAllTransactionForGroup(groupID);
                        _studentAcceptanceDAO.DeleteRecordForStudent(studentID);
                    }
                    else
                    {
                        _studentAcceptanceDAO.DeleteRecordForStudent(studentID);
                    }
                    break;
            }

        }
        private bool CheckAmountOfAcceptance(int groupID)
        {
            var currentTransaction = _groupTrasanctionDAO.FindOneRecordBy(groupID);
            var currentAmountOfAcceptance = currentTransaction.numberOfAcceptance + 1;
            currentTransaction.numberOfAcceptance = currentAmountOfAcceptance;
            _groupTrasanctionDAO.UpdateRecord(currentTransaction);
            if (currentTransaction.numberOfAcceptance == currentTransaction.numberOfStudents)
            {
                return true;
            }
            return false;
        }

        private void CreateAndAddNewRecordGroupArtifact(int groupID, int artifactID)
        {
            var model = new OwnedArtifactGroupDAO();
            var newRecord = new OwnedArtifactGroup()
            {
                GroupId = groupID,
                ArtifactId = artifactID,
                CompletionStatus = 0,
            };
            model.AddRecord(newRecord);
        }

        private void UpdateValueStudentWallet(int groupID, int artifactID)
        {
            var studentGroup = new GroupDAO().FindOneRecordBy(groupID);
            studentGroup.GroupStudents = new StudentDAO().FetchAllStudentInGroup(groupID);
            var artifactToBuy = new ArtifactDAO().FindOneRecordBy(artifactID);
            int amountStudents = studentGroup.GroupStudents.Count;

            foreach (Student student in studentGroup.GroupStudents)
            {
                int currentWalletValue = student.Wallet - (artifactToBuy.Cost / amountStudents);
                student.Wallet = currentWalletValue;
                new StudentDAO().UpdateRecord(student);
            }
        }

        public enum answear
        {
            No = 1,
            Yes = 2
        }
    }


}
