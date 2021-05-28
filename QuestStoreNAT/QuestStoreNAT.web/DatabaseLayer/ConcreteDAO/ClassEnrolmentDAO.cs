using System;
using QuestStoreNAT.web.Models;
using Npgsql;
using Npgsql.PostgresTypes;
using System.Collections.Generic;

namespace QuestStoreNAT.web.DatabaseLayer
{
    public class ClassEnrolmentDAO:DBAbstractRecord<ClassEnrollment>
    {
        public override string DBTableName { get; set; } = "ClassEnrollment";
        

        public List<ClassEnrollment> FetchAllRecordsJoin()
        {
            using NpgsqlConnection connection = OpenConnectionToDB();
            var query = ProvideQueryINNER();
            using var command = new NpgsqlCommand(query , connection);
            var reader = command.ExecuteReader();

            var allRecords = new List<ClassEnrollment>();
            while ( reader.Read() )
            {
                allRecords.Add(ProvideOneRecord(reader));
            };
            return allRecords;
        }


        public override ClassEnrollment ProvideOneRecord( NpgsqlDataReader reader )
        {
            var classEnrol = new ClassEnrollment();
            classEnrol.Id = reader.GetInt32(0);
            classEnrol.MentorCE = new Mentor
            {
                Id = reader.GetInt32(1),
                FirstName = reader.GetString(2) ,
                LastName = reader.GetString(3)
            };
            classEnrol.ClassroomCE = new Classroom
            {
                Id = reader.GetInt32(4) ,
                Name = reader.GetString(5)
            };

            return classEnrol;
        }

        private string ProvideQueryINNER()
        {
            string query = "SELECT CE.\"ID\", ME.\"ID\", ME.\"FirstName\", ME.\"Surname\", CS.\"ID\", CS.\"Name\"" +
                "FROM \"NATQuest\".\"ClassEnrollment\" CE " +
                "INNER JOIN \"NATQuest\".\"Mentors\" ME on CE.\"MentorID\" = ME.\"ID\"" +
                "INNER JOIN \"NATQuest\".\"Classes\" CS on CE.\"ClassID\" = CS.\"ID\"" +
                "order by ME.\"ID\";";
            return query;
        }


        public override string ProvideQueryStringToAdd( ClassEnrollment classEnrollmentToAdd )
        {
            string query = $"UPDATE \"NATQuest\".\"{DBTableName}\"" +
                $"SET \"ClassID\" = {classEnrollmentToAdd.ClassroomCE.Id}" +
                $"SET \"MentorID\" = {classEnrollmentToAdd.MentorCE.Id}" +
                $"WHERE \"ID\" = {classEnrollmentToAdd.Id};";
            return query;
        }

        public override string ProvideQueryStringToUpdate( ClassEnrollment recordToUpdate )
        {
            throw new NotImplementedException();
        }
    }
}
