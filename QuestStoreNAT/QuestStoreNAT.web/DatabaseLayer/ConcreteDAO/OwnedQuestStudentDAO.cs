using Npgsql;
using System.Collections.Generic;
using QuestStoreNAT.web.Models;

namespace QuestStoreNAT.web.DatabaseLayer
{
    public class OwnedQuestStudentDAO : DBAbstractRecord<OwnedQuestStudent>
    {
        public override string DBTableName { get; set; } = "OwnedQuestStudent";

        private enum OwnedQuestStudentEnum
        {
            Id, StudentId, QuestId, QuestStatusId
        }

        public override OwnedQuestStudent ProvideOneRecord(NpgsqlDataReader reader)
        {
            return new OwnedQuestStudent
            {
                Id = reader.GetInt32((int)OwnedQuestStudentEnum.Id),
                StudentId = reader.GetInt32((int)OwnedQuestStudentEnum.StudentId),
                QuestId = reader.GetInt32((int)OwnedQuestStudentEnum.QuestId),
                CompletionStatus = (CompletionStatus)reader.GetInt32((int)OwnedQuestStudentEnum.QuestStatusId)
            };
        }

        public override string ProvideQueryStringToAdd(OwnedQuestStudent recordToAdd)
        {
            var query = $"INSERT INTO \"NATQuest\".\"{DBTableName}\" (\"StudentID\", \"QuestID\", \"QuestStatusID\")" +
                        $"VALUES({recordToAdd.StudentId}, " +
                        $"{recordToAdd.QuestId}, " +
                        $"{(int)recordToAdd.CompletionStatus});";
            return query;
        }

        public override string ProvideQueryStringToUpdate(OwnedQuestStudent ownedQuestToUpdate)
        {
            var query = $"UPDATE \"NATQuest\".\"{DBTableName}\" " +
                        $"SET \"QuestStatusID\" = {(int)ownedQuestToUpdate.CompletionStatus}"+
                        $"WHERE \"ID\" = {ownedQuestToUpdate.Id};";
            return query;
        }

        public List<OwnedQuestStudent> FetchAllRecords(int studentID)
        {
            using NpgsqlConnection connection = OpenConnectionToDB();
            var query = $"SELECT * FROM \"NATQuest\".\"{DBTableName}\" WHERE \"StudentID\" = '{studentID}';";
            using var command = new NpgsqlCommand(query, connection);
            var reader = command.ExecuteReader();

            var allRecords = new List<OwnedQuestStudent>();
            while (reader.Read())
            {
                allRecords.Add(ProvideOneRecord(reader));
            };
            return allRecords;
        }
    }
}
