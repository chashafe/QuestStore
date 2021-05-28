using System;
using Npgsql;
using QuestStoreNAT.web.Models;
using System.Collections.Generic;

namespace QuestStoreNAT.web.DatabaseLayer
{
    public class StudentDAO : DBAbstractRecord_WithCredentials<Student>
    {
        public override string DBTableName { get; set; } = "Students";

        private enum StudentEnum
        {
            Id, ClassId, GroupId, FirstName, Surname, CoinsTotal, CoinsBalance, CredentialID
        }


        public override Student ProvideOneRecord(NpgsqlDataReader reader)
        {
            var student = new Student();
            student.Id = reader.GetInt32((int)StudentEnum.Id);
            student.ClassID = reader.GetInt32((int)StudentEnum.ClassId);
            student.GroupID = reader.GetInt32((int)StudentEnum.GroupId);
            student.FirstName = reader.GetString((int)StudentEnum.FirstName);
            student.LastName = reader.GetString((int)StudentEnum.Surname);
            student.Wallet = reader.GetInt32((int)StudentEnum.CoinsTotal);
            student.OverallWalletLevel = reader.GetInt32((int)StudentEnum.CoinsBalance);
            student.CredentialID = reader.GetInt16((int)StudentEnum.CredentialID);
            return student;
        }

        public override Student FindOneRecordBy(int id)
        {
            using NpgsqlConnection connection = OpenConnectionToDB();
            var query = $"SELECT * FROM \"NATQuest\".\"{DBTableName}\" WHERE \"Credential_ID\" = '{id}';";
            using var command = new NpgsqlCommand(query, connection);
            var reader = command.ExecuteReader();

            var oneRecord = default(Student);
            while (reader.Read())
            {
                oneRecord = ProvideOneRecord(reader);
            };
            return oneRecord;
        }

        public override string ProvideQueryStringToAdd(Student studentToAdd)
        {
            throw new System.NotImplementedException();
        }

        public override string ProvideQueryStringToUpdate(Student studentToUpdate)
        {
            var query = $"UPDATE \"NATQuest\".\"{DBTableName}\" " +
                        $"SET \"ClassID\" = {studentToUpdate.ClassID}," + 
                        $"\"GroupID\" = {studentToUpdate.GroupID}," +
                        $"\"FirstName\" = '{studentToUpdate.FirstName}', " +
                        $"\"Surname\" = '{studentToUpdate.LastName}'," +
                        $"\"CoinsTotal\" = {studentToUpdate.Wallet}," +
                        $"\"CoinsBalance\" = {studentToUpdate.OverallWalletLevel}" +
                        $"WHERE \"ID\" = {studentToUpdate.Id};";
            return query;
        }


        internal List<Student> FetchAllRecordsByIdJoin( int id)
        {
            using NpgsqlConnection connection = OpenConnectionToDB();
            var query = ProvideQueryToGetStudentsAssignedToMentor(id);
            using var command = new NpgsqlCommand(query , connection);
            var reader = command.ExecuteReader();

            var allRecords = new List<Student>();
            while ( reader.Read() )
            {
                allRecords.Add(ProvideOneRecord(reader));
            };
            return allRecords;
        }

        private string ProvideQueryToGetStudentsAssignedToMentor(int id)
        {
            var query = $"select ST.*" +
                        $"FROM(select * " +
                        $"from \"NATQuest\".\"ClassEnrollment\" CE " +
                        $"inner join \"NATQuest\".\"Mentors\" ME " +
                        $"on CE.\"MentorID\" = ME.\"ID\" " +
                        $"inner join \"NATQuest\".\"Classes\" CL " +
                        $"on CE.\"ClassID\" = CL.\"ID\") as klasy " +
                        $"inner join \"NATQuest\".\"Students\" ST " +
                        $"on klasy.\"ClassID\" = ST.\"ClassID\"; " +
                        $"WHERE klasy.\"MentorID\" = {id};";
            return query;
        }
        

        /*
        public override string ProvideQueryStringToUpdate(Student studentToUpdate)
        {
            var query = $"UPDATE \"NATQuest\".\"{DBTableName}\" " +
                        $"SET \"CoinsTotal\" = {studentToUpdate.Wallet} " +
                        $"WHERE \"NATQuest\".\"{DBTableName}\".\"ID\" = {studentToUpdate.Id};";
            return query;
        }
        */

        public override void UpdateRecord( Student studentToUpdate )
        {
            using NpgsqlConnection connection = OpenConnectionToDB();
            string query = ProvideQueryStringToUpdate(studentToUpdate);
            ExecuteQuery(connection , query);
        }

        #region outOfAbstraction
        public string ProvideQueryStringReturningID(Student studentToAdd)
        {
            var query = $"INSERT INTO \"NATQuest\".\"{DBTableName}\" " +
                        $"(\"ClassID\", \"GroupID\", \"Credential_ID\", \"FirstName\", \"Surname\", \"CoinsBalance\", \"CoinsTotal\")" +
                        $"VALUES({studentToAdd.ClassID}, {studentToAdd.GroupID}, {studentToAdd.CredentialID}," +
                              $"'{studentToAdd.FirstName}','{studentToAdd.LastName}'," +
                              $"{studentToAdd.Wallet}," +
                              $"{studentToAdd.OverallWalletLevel}) RETURNING \"ID\";";
            return query;
        }

        public int AddStudentByCredentialsReturningID( int credentialID ) // credentialID from CredentialsDAO.AddRecordReturningID(Credentials newCredential)
        {
            var newStudent = new Student
            {
                ClassID = 200,
                GroupID = 200,
                CredentialID = credentialID,
                FirstName = "" ,
                LastName = "" ,
                Wallet = 0 ,
                OverallWalletLevel = 0
            };

            using NpgsqlConnection connection = OpenConnectionToDB();
            string query = ProvideQueryStringReturningID(newStudent);
            return ExecuteScalar(connection , query); // StudentID used to instanly update Student
        }

        private int ExecuteScalar( NpgsqlConnection connection , string query )
        {
            using var command = new NpgsqlCommand(query , connection);
            command.Prepare();
            return Convert.ToInt32(command.ExecuteScalar());
        }
        #endregion

        public List<Student> FetchAllStudentInGroup(int groupID)
        {
            using NpgsqlConnection connection = OpenConnectionToDB();
            var query = $"SELECT * FROM \"NATQuest\".\"Students\" WHERE \"NATQuest\".\"Students\".\"GroupID\" = '{groupID}';";
            using var command = new NpgsqlCommand(query, connection);
            var reader = command.ExecuteReader();

            var allRecords = new List<Student>();
            while (reader.Read())
            {
                allRecords.Add(ProvideOneRecord(reader));
            };
            return allRecords;
        }

    }
}
