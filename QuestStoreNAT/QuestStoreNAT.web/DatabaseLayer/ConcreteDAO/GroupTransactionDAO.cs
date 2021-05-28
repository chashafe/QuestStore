using System;
using QuestStoreNAT.web.Models;
using Npgsql;
namespace QuestStoreNAT.web.DatabaseLayer
{
    public class GroupTransactionDAO : DBAbstractRecord<GroupTransaction>
    {
        public override string DBTableName { get; set; } = "GroupChosenArtifacts";
        private enum GroupTransactionEnum
        {
            Id, artifactID, groupID, numberOfStudent, numberOfAcceptance
        }

        public override GroupTransaction ProvideOneRecord(NpgsqlDataReader reader)
        {
            var groupTransaction = new GroupTransaction();
            groupTransaction.ID = reader.GetInt32((int)GroupTransactionEnum.Id);
            groupTransaction.artifactID = reader.GetInt32((int)GroupTransactionEnum.artifactID);
            groupTransaction.groupID = reader.GetInt32((int)GroupTransactionEnum.groupID);
            groupTransaction.numberOfStudents = reader.GetInt32((int)GroupTransactionEnum.numberOfStudent);
            groupTransaction.numberOfAcceptance = reader.GetInt32((int)GroupTransactionEnum.numberOfAcceptance);
            return groupTransaction;
        }

        public override string ProvideQueryStringToAdd(GroupTransaction groupTransactionToAdd)
        {
            var query = $"INSERT INTO \"NATQuest\".\"{DBTableName}\" (\"ArtifactID\", \"GroupID\", \"NumberOfStudents\", \"NumberOfAcceptances\")" +
                        $"VALUES({groupTransactionToAdd.artifactID}, " +
                               $"'{groupTransactionToAdd.groupID}', " +
                               $"'{groupTransactionToAdd.numberOfStudents}', " +
                               $"'{groupTransactionToAdd.numberOfAcceptance}');";
            return query;
        }

        public override string ProvideQueryStringToUpdate(GroupTransaction groupTransactionToUpdate)
        {
            var query = $"UPDATE \"NATQuest\".\"{DBTableName}\" " +
                        $"SET \"ArtifactID\" = {(int)groupTransactionToUpdate.artifactID}, " +
                            $"\"GroupID\" = '{groupTransactionToUpdate.groupID}', " +
                            $"\"NumberOfStudents\" = '{groupTransactionToUpdate.numberOfStudents}', " +
                            $"\"NumberOfAcceptances\" = '{groupTransactionToUpdate.numberOfAcceptance}' " +
                        $"WHERE \"NATQuest\".\"{DBTableName}\".\"ID\" = {groupTransactionToUpdate.ID};";
            return query;
        }

        public override GroupTransaction FindOneRecordBy(int id)
        {
            using NpgsqlConnection connection = OpenConnectionToDB();
            var query = $"SELECT * FROM \"NATQuest\".\"{DBTableName}\" WHERE \"NATQuest\".\"{DBTableName}\".\"GroupID\" = '{id}' LIMIT 1;";
            using var command = new NpgsqlCommand(query, connection);
            var reader = command.ExecuteReader();

            var oneRecord = default(GroupTransaction);
            while (reader.Read())
            {
                oneRecord = ProvideOneRecord(reader);
            };
            return oneRecord;
        }

        public void DeleteAllTransactionForGroup(int id)
        {
            using NpgsqlConnection connection = OpenConnectionToDB();
            string query = $"DELETE FROM \"NATQuest\".\"{DBTableName}\" WHERE \"NATQuest\".\"{DBTableName}\".\"GroupID\" = {id}";
            ExecuteQuery(connection, query);
        }
    }
}
