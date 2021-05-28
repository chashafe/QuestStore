using System;
using QuestStoreNAT.web.Models;
using QuestStoreNAT.web.DatabaseLayer;

namespace QuestStoreNAT.web.Services
{
    public class ArtifactManagement
    {
        private StudentDAO _studentDAO { get; set; }
        private ArtifactDAO _artifact { get; set; }
        private OwnedArtifactGroupDAO _ownedArtifactGroup { get; set; }
        private OwnedArtifactStudentDAO _ownedArtifactStudent { get; set; }
        private GroupDAO _groupDAO { get; set; }
        private Student student { get; set; }
        private GroupTransactionDAO _groupTransactionDAO { get; set; }
        private StudentAcceptanceDAO _studentAcceptanceDAO { get; set; }

        public ArtifactManagement(IDB_GenericInterface<GroupTransaction> groupTranscation)
        {
            _groupTransactionDAO = groupTranscation as GroupTransactionDAO;
        }
        public ArtifactManagement()
        {
            _studentDAO = new StudentDAO();
            _artifact = new ArtifactDAO();
            _ownedArtifactGroup = new OwnedArtifactGroupDAO();
            _ownedArtifactStudent = new OwnedArtifactStudentDAO();
            _groupDAO = new GroupDAO();
            _groupTransactionDAO = new GroupTransactionDAO();
            _studentAcceptanceDAO = new StudentAcceptanceDAO();
        }

        public void UseArtifact(Student student, int artifactID)
        {
            var artifactToUse = _artifact.FindOneRecordBy(artifactID);
            CompletionStatus completionStatus = CompletionStatus.Unfinished;
            switch(artifactToUse.Type)
            {
                case TypeClassification.Individual:
                    var model =_ownedArtifactStudent.FindOneRecordBy(artifactID, student.Id, completionStatus);
                    model.CompletionStatus = CompletionStatus.Finished;
                    _ownedArtifactStudent.UpdateRecord(model);
                    break;
                case TypeClassification.Group:
                    var modelGroup = _ownedArtifactGroup.FindOneRecordBy(artifactID, student.GroupID, completionStatus);
                    modelGroup.CompletionStatus = CompletionStatus.Finished;
                    _ownedArtifactGroup.UpdateRecord(modelGroup);
                    break;
            }
        }

        public void DeleteUsedArtifactFromView(Student student, int artifactID)
        {
            var artifactToDelete = _artifact.FindOneRecordBy(artifactID);
            CompletionStatus completionStatus = CompletionStatus.Finished;
            switch (artifactToDelete.Type)
            {
                case TypeClassification.Individual:
                    var model = _ownedArtifactStudent.FindOneRecordBy(artifactID, student.Id, completionStatus);
                    _ownedArtifactStudent.DeleteRecord(model.Id);
                    break;
                case TypeClassification.Group:
                    var modelGroup = _ownedArtifactGroup.FindOneRecordBy(artifactID, student.GroupID, completionStatus);
                    _ownedArtifactGroup.DeleteRecord(modelGroup.Id);
                    break;
            }
        }

        public void BuyIndiviudalArtifact(int credentialID, int artifactID)
        {
            var currentStudent = _studentDAO.FindOneRecordBy(credentialID);
            var artifactToBuy = _artifact.FindOneRecordBy(artifactID);
            var newRecord = new OwnedArtifactStudent()
            {
                StudentId = currentStudent.Id,
                ArtifactId = artifactID,
                CompletionStatus = CompletionStatus.Unfinished,
            };
            int currentWalletValue = currentStudent.Wallet - artifactToBuy.Cost;
            currentStudent.Wallet = currentWalletValue;
            UpdateGroupWallet(currentStudent.GroupID, artifactToBuy.Cost);
            _studentDAO.UpdateRecord(currentStudent);
            _ownedArtifactStudent.AddRecord(newRecord);
        }

        public bool CheckigStudentWallet(int credentialID, int artifactID)
        {
            var currentStudent = _studentDAO.FindOneRecordBy(credentialID).Wallet;
            var artifactToBuy = _artifact.FindOneRecordBy(artifactID).Cost;
            if (currentStudent < artifactToBuy)
            {
                return false;
            }
            return true;
        }

        public bool CheckigGroupWallet(int groupID, int artifactID, int credentialID)
        {
            int currentValueGroupWallet = _groupDAO.FindOneRecordBy(groupID).GroupWallet;
            var artifactToBuy = _artifact.FindOneRecordBy(artifactID).Cost;

            if (currentValueGroupWallet > artifactToBuy && CheckingStudentToBoughtGroupArtifact(groupID,credentialID,artifactID) == true)
            {
                return true;
            }
            return false;
        }

        private bool CheckingStudentToBoughtGroupArtifact(int groupID, int credentialID, int artifactID)
        {
            var currentStudent = _studentDAO.FindOneRecordBy(credentialID);
            var studentGroup = _groupDAO.FindOneRecordBy(currentStudent.GroupID);
            var artifactToBuy = _artifact.FindOneRecordBy(artifactID);
            studentGroup.GroupStudents = _studentDAO.FetchAllStudentInGroup(groupID);
            int amountStudents = studentGroup.GroupStudents.Count;
            if(currentStudent.Wallet - (artifactToBuy.Cost / amountStudents) > 0)
            {
                return true;
            }
            return false;
        }

        public void UpdateGroupWallet(int studentGroup, int artifactCost)
        {
            int currentValueGroupWallet = _groupDAO.FindOneRecordBy(studentGroup).GroupWallet;
            int ValueGroupWalletAfterBought = currentValueGroupWallet - artifactCost;
            _groupDAO.UpdateOnlyGroupWallet(studentGroup, ValueGroupWalletAfterBought);
        }

        public bool CheckingIfTransactionForBoughtGroupArtifactExist(int groupID)
        {
            if (_groupTransactionDAO.FindOneRecordBy(groupID) != null)
            {
                return false;
            }
            return true;
        }

        public void CreateNewGroupTransaction(int artifactID, int GroupID)
        {
            var studentGroup = _groupDAO.FindOneRecordBy(GroupID);
            studentGroup.GroupStudents = _studentDAO.FetchAllStudentInGroup(GroupID);
            int amountOfStudentInGroup = studentGroup.GroupStudents.Count;
            var newRecordGroupTransaction = new GroupTransaction()
            {
                artifactID = artifactID,
                groupID = GroupID,
                numberOfStudents = amountOfStudentInGroup,
                numberOfAcceptance = 1
            };
            _groupTransactionDAO.AddRecord(newRecordGroupTransaction);
        }

        public void CreateRecordForAcceptance(int credentialID, int groupID, int artifactID)
        {
            var currentStudent = _studentDAO.FindOneRecordBy(credentialID);
            var studentGroup = _groupDAO.FindOneRecordBy(groupID);
            studentGroup.GroupStudents = _studentDAO.FetchAllStudentInGroup(groupID);

            foreach (Student student in studentGroup.GroupStudents)
            {
                if (student.Id != currentStudent.Id)
                {
                    var newStudentAcceptance = new StudentAcceptance()
                    {
                        studentID = student.Id,
                        artifactID = artifactID,
                        acceptance = 0,
                        groupID = groupID
                    };
                    _studentAcceptanceDAO.AddRecord(newStudentAcceptance);
                }
            }
        }
    }
}
