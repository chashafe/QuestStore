using System.Collections.Generic;
using Npgsql;
using QuestStoreNAT.web.Models;


namespace QuestStoreNAT.web.DatabaseLayer
{
    public class GroupDAO : DBAbstractRecord<Group>
    {
        public override string DBTableName { get; set; } = "Groups";

        private enum GroupEnum
        {
            Id, ClassroomID, Name, GroupWallet
        }

        public List<Group> FetchAllRecordsByIdJoin( int id )
        {
            using NpgsqlConnection connection = OpenConnectionToDB();
            var query = ProvideQueryToGetGroupAssignedToMentor(id);
            using var command = new NpgsqlCommand(query , connection);
            var reader = command.ExecuteReader();

            var allRecords = new List<Group>();
            while ( reader.Read() )
            {
                allRecords.Add(ProvideOneRecord(reader));
            };
            return allRecords;
        }

        public override Group ProvideOneRecord( NpgsqlDataReader reader )
        {
            var group = new Group()
            {
                Id = reader.GetInt32((int)GroupEnum.Id),
                ClassroomId = reader.GetInt32((int)GroupEnum.ClassroomID),
                Name = reader.GetString((int)GroupEnum.Name),
                GroupWallet = reader.GetInt32((int)GroupEnum.GroupWallet)
            };
            return group;
        }

        public override string ProvideQueryStringToAdd( Group groupToAdd )
        {
            var query = $"INSERT INTO \"NATQuest\".\"{DBTableName}\" (\"ClassID\", \"Name\", \"CoinsTotal\")" +
                        $"VALUES({groupToAdd.ClassroomId}," +
                        $"'{groupToAdd.Name}'," +
                        $"{groupToAdd.GroupWallet});";
            return query;
        }

        public override string ProvideQueryStringToUpdate( Group groupToAdd )
        {
            var query = $"UPDATE \"NATQuest\".\"{DBTableName}\" " +
                 $"SET \"ClassID\" = {groupToAdd.ClassroomId}, " +
                     $"\"Name\" = '{groupToAdd.Name}', " +
                     $"\"CoinsTotal\" = '{groupToAdd.GroupWallet}'" +
                 $"WHERE \"NATQuest\".\"{DBTableName}\".\"ID\" = {groupToAdd.Id};";
            return query;
        }

        public void UpdateOnlyGroupWallet(int groupID, int coinsTotal)
        {

            using NpgsqlConnection connection = OpenConnectionToDB();
            var query = $"UPDATE \"NATQuest\".\"{DBTableName}\" " +
                 $"SET \"CoinsTotal\" = {coinsTotal}" +
                 $"WHERE \"NATQuest\".\"{DBTableName}\".\"ID\" = {groupID};";

            ExecuteQuery(connection, query);

        }

        private string ProvideQueryToGetGroupAssignedToMentor(int id)
        {
            var query = $"SELECT grupy.\"GroupID\", grupy.\"ClassroomID\", " +
                $"grupy.\"Name\", grupy.\"GroupWallet\" " +
                $"FROM(SELECT * from \"NATQuest\".\"ClassEnrollment\" CE " +
                $"INNER JOIN \"NATQuest\".\"Mentors\" ME ON CE.\"MentorID\" = ME.\"ID\"" +
                $"INNER JOIN \"NATQuest\".\"Classes\" CL ON CE.\"ClassID\" = CL.\"ID\") AS klasy " +
                $"INNER JOIN (SELECT GR.\"ID\" AS \"GroupID\", GR.\"ClassID\" AS \"ClassroomID\", " +
                $"GR.\"Name\" AS \"Name\", GR.\"CoinsTotal\" AS \"GroupWallet\" " +
                $"FROM \"NATQuest\".\"Groups\" GR INNER JOIN \"NATQuest\".\"Students\" ST " +
                $"ON GR.\"ClassID\" = ST.\"ClassID\") AS grupy " +
                $"ON klasy.\"ClassID\" = grupy.\"ClassroomID\" " +
                $"WHERE klasy.\"MentorID\" = {id};"; // 
            return query;
        }
    }
}
