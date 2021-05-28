using System.Collections.Generic;
using System.Linq;
using QuestStoreNAT.web.Models;
using QuestStoreNAT.web.DatabaseLayer;
using QuestStoreNAT.web.DatabaseLayer.ConcreteDAO;
using QuestStoreNAT.web.ViewModels;

namespace QuestStoreNAT.web.Services
{
    public class QuestManagement
    {
        private OwnedQuestStudentDAO _ownedStudentDAO { get; set; }
        private OwnedQuestGroupDAO _ownedGroupDAO { get; set; }
        private QuestDAO _questDAO { get; set; }

        public QuestManagement()
        {
            _ownedStudentDAO = new OwnedQuestStudentDAO();
            _questDAO = new QuestDAO();
            _ownedGroupDAO = new OwnedQuestGroupDAO();
        }

        private List<OwnedQuestIdWithQuest> ReturnAllIndividualQuest(int studentId)
        {
            var ownedQuestIdWithQuestList = new List<OwnedQuestIdWithQuest>();

            var allOwnedQuestByStudent = _ownedStudentDAO.FetchAllRecords(studentId);

            foreach(var ownedQuestByStudent in allOwnedQuestByStudent)
            {
                var ownedQuestIdWithQuest = new OwnedQuestIdWithQuest {OwnedId = ownedQuestByStudent.Id};

                var model = _questDAO.FindOneRecordBy(ownedQuestByStudent.QuestId);
                model.QuestStatus = ownedQuestByStudent.CompletionStatus;
                ownedQuestIdWithQuest.OwnedQuest = model;

                ownedQuestIdWithQuestList.Add(ownedQuestIdWithQuest);
            }
            return ownedQuestIdWithQuestList;
        }

        private List<OwnedQuestIdWithQuest> ReturnAllGroupQuest(int groupId)
        {
            var ownedQuestIdWithQuestList = new List<OwnedQuestIdWithQuest>();

            var allOwnedQuestByGroup = _ownedGroupDAO.FetchAllRecords(groupId);

            foreach (var ownedQuestByGroup in allOwnedQuestByGroup)
            {
                var ownedQuestIdWithQuest = new OwnedQuestIdWithQuest {OwnedId = ownedQuestByGroup.Id};

                var model = _questDAO.FindOneRecordBy(ownedQuestByGroup.QuestId);
                model.QuestStatus = ownedQuestByGroup.CompletionStatus;
                ownedQuestIdWithQuest.OwnedQuest = model;

                ownedQuestIdWithQuestList.Add(ownedQuestIdWithQuest);
            }
            return ownedQuestIdWithQuestList;
        }

        public List<OwnedQuestIdWithQuest> ReturnListOfAllQuest(int studentId, int groupId)
        {
            var listIndividualQuest = ReturnAllIndividualQuest(studentId);
            var listGroupQuest = ReturnAllGroupQuest(groupId);

            return listIndividualQuest.Concat(listGroupQuest).ToList();
        }

        public void ClaimIndividualQuest(OwnedQuestStudent claimedOwnedQuest)
        {
            _ownedStudentDAO.AddRecord(claimedOwnedQuest);
        }

        public void ClaimGroupQuest(OwnedQuestGroup claimedOwnedQuest)
        {
            _ownedGroupDAO.AddRecord(claimedOwnedQuest);
        }

        public void DeclaimIndividualQuest(int id)
        {
            _ownedStudentDAO.DeleteRecord(id);
        }

        public void DeclaimGroupQuest(int id)
        {
            _ownedGroupDAO.DeleteRecord(id);
        }
    }
}
