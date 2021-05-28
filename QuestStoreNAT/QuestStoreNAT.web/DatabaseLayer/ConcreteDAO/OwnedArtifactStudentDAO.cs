using System;
using Npgsql;
using QuestStoreNAT.web.Models;

namespace QuestStoreNAT.web.DatabaseLayer
{
    public class OwnedArtifactStudentDAO : DBAbstractRecord<OwnedArtifactStudent>
    {
        public override string DBTableName { get; set; } = "OwnedArtifactStudent";
        private enum OwnedArtifactStudentEnum
        {
            Id, StudentId, ArtifactId,  CompletionStatus,
        }

        public override OwnedArtifactStudent ProvideOneRecord(NpgsqlDataReader reader)
        {
            var ownedArtifactStudent = new OwnedArtifactStudent();

            ownedArtifactStudent.Id = reader.GetInt32((int)OwnedArtifactStudentEnum.Id);
            ownedArtifactStudent.StudentId = reader.GetInt32((int)OwnedArtifactStudentEnum.StudentId);
            ownedArtifactStudent.ArtifactId = reader.GetInt32((int)OwnedArtifactStudentEnum.ArtifactId);
            ownedArtifactStudent.CompletionStatus = (CompletionStatus)reader.GetInt32((int)OwnedArtifactStudentEnum.CompletionStatus);
            return ownedArtifactStudent;
        }

        public override string ProvideQueryStringToAdd(OwnedArtifactStudent OwnedArtifactStudentToAdd)
        {
            var query = $"INSERT INTO \"NATQuest\".\"{DBTableName}\" (\"StudentID\", \"ArtifactID\", \"ArtifactStatusID\")" +
                        $"VALUES({OwnedArtifactStudentToAdd.StudentId}, " +
                               $"'{OwnedArtifactStudentToAdd.ArtifactId}', " +
                               $"'{(int)OwnedArtifactStudentToAdd.CompletionStatus}');";
            return query;
        }

        public override string ProvideQueryStringToUpdate(OwnedArtifactStudent OwnedArtifactStudentToUpdate)
        {
            var query = $"UPDATE \"NATQuest\".\"{DBTableName}\" " +
                        $"SET \"StudentID\" = {OwnedArtifactStudentToUpdate.StudentId}, " +
                            $"\"ArtifactID\" = '{OwnedArtifactStudentToUpdate.ArtifactId}', " +
                            $"\"ArtifactStatusID\" = '{(int)OwnedArtifactStudentToUpdate.CompletionStatus}' " +
                        $"WHERE \"NATQuest\".\"{DBTableName}\".\"ID\" = {OwnedArtifactStudentToUpdate.Id};";
            return query;
        }

        public OwnedArtifactStudent FindOneRecordBy(int id, int studentID, CompletionStatus completiotStatus)
        {
            using NpgsqlConnection connection = OpenConnectionToDB();
            var query = $"SELECT * FROM \"NATQuest\".\"{DBTableName}\" WHERE \"ArtifactID\" = '{id}' AND \"StudentID\" = '{studentID}' AND \"ArtifactStatusID\" = {(int)completiotStatus} LIMIT 1;";
            using var command = new NpgsqlCommand(query, connection);
            var reader = command.ExecuteReader();

            var oneRecord = default(OwnedArtifactStudent);
            while (reader.Read())
            {
                oneRecord = ProvideOneRecord(reader);
            };
            return oneRecord;
        }

    }
}