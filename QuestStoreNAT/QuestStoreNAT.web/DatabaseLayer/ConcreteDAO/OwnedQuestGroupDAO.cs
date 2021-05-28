using Npgsql;
using QuestStoreNAT.web.Models;
using System.Collections.Generic;

namespace QuestStoreNAT.web.DatabaseLayer.ConcreteDAO
{
    public class OwnedQuestGroupDAO : DBAbstractRecord<OwnedQuestGroup>
    {
        public override string DBTableName { get; set; } = "OwnedQuestGroup";

        private enum OwnedQuestGroupEnum
        {
            Id, GroupId, QuestId, QuestStatusId
        }

        public override OwnedQuestGroup ProvideOneRecord(NpgsqlDataReader reader)
        {
            return new OwnedQuestGroup()
            {
                Id = reader.GetInt32((int)OwnedQuestGroupEnum.Id),
                GroupId = reader.GetInt32((int)OwnedQuestGroupEnum.GroupId),
                QuestId = reader.GetInt32((int)OwnedQuestGroupEnum.QuestId),
                CompletionStatus = (CompletionStatus)reader.GetInt32((int)OwnedQuestGroupEnum.QuestStatusId)
            };
        }

        public override string ProvideQueryStringToAdd(OwnedQuestGroup recordToAdd)
        {
            var query = $"INSERT INTO \"NATQuest\".\"{DBTableName}\" (\"GroupID\", \"QuestID\", \"QuestStatusID\")" +
                        $"VALUES({recordToAdd.GroupId}, " +
                        $"{recordToAdd.QuestId}, " +
                        $"{(int)recordToAdd.CompletionStatus});";
            return query;
        }

        public override string ProvideQueryStringToUpdate(OwnedQuestGroup recordToUpdate)
        {
            var query = $"UPDATE \"NATQuest\".\"{DBTableName}\" " +
                        $"SET \"QuestStatusID\" = {(int)recordToUpdate.CompletionStatus}" +
                        $"WHERE \"ID\" = {recordToUpdate.Id};";
            return query;
        }

        public List<OwnedQuestGroup> FetchAllRecords(int groupID)
        {
            using NpgsqlConnection connection = OpenConnectionToDB();
            var query = $"SELECT * FROM \"NATQuest\".\"{DBTableName}\" WHERE \"GroupID\" = '{groupID}';";
            using var command = new NpgsqlCommand(query, connection);
            var reader = command.ExecuteReader();

            var allRecords = new List<OwnedQuestGroup>();
            while (reader.Read())
            {
                allRecords.Add(ProvideOneRecord(reader));
            };
            return allRecords;

        }
    }
}
