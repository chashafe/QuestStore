using System;
using QuestStoreNAT.web.Models;
using System.Collections.Generic;
using Npgsql;

namespace QuestStoreNAT.web.DatabaseLayer
{
    public class ArtifactDAO : DBAbstractRecord<Artifact>
    {
        public override string DBTableName { get; set; } = "Artifacts";
        private enum ArtifactEnum
        {
            Id, ArtifactTypeID, Name, Cost, Description
        }

        public override Artifact ProvideOneRecord(NpgsqlDataReader reader)
        {
            var artifact = new Artifact();
            artifact.Id = reader.GetInt32((int)ArtifactEnum.Id);
            artifact.Type = (TypeClassification)reader.GetInt32((int)ArtifactEnum.ArtifactTypeID);
            artifact.Name = reader.GetString((int)ArtifactEnum.Name);
            artifact.Cost = reader.GetInt32((int)ArtifactEnum.Cost);
            artifact.Description = reader.GetString((int)ArtifactEnum.Description);
            return artifact;
        }

        public override string ProvideQueryStringToAdd(Artifact artifactToAdd)
        {
            var query = $"INSERT INTO \"NATQuest\".\"{DBTableName}\" (\"ArtifactTypeID\", \"Name\", \"Cost\", \"Description\")" +
                        $"VALUES({(int)artifactToAdd.Type}, " +
                               $"'{artifactToAdd.Name}', " +
                               $"{artifactToAdd.Cost}, " +
                               $"'{artifactToAdd.Description}');";
            return query;
        }

        public override string ProvideQueryStringToUpdate(Artifact artifactToUpdate)
        {
            var query = $"UPDATE \"NATQuest\".\"{DBTableName}\" " +
                        $"SET \"ArtifactTypeID\" = {(int)artifactToUpdate.Type}, " +
                            $"\"Name\" = '{artifactToUpdate.Name}', " +
                            $"\"Cost\" = '{artifactToUpdate.Cost}', " +
                            $"\"Description\" = '{artifactToUpdate.Description}'" +
                        $"WHERE \"NATQuest\".\"{DBTableName}\".\"ID\" = {artifactToUpdate.Id};";
            return query;
        }

        public List<Artifact> FetchAllRecords(int id, int statusArtifact)
        {
            using NpgsqlConnection connection = OpenConnectionToDB();
            var query = $"SELECT \"NATQuest\".\"Artifacts\".\"ID\",\"NATQuest\".\"OwnedArtifactStudent\".\"ArtifactStatusID\", \"NATQuest\".\"Artifacts\".\"Name\", \"NATQuest\".\"Artifacts\".\"Cost\", \"NATQuest\".\"Artifacts\".\"Description\" FROM \"NATQuest\".\"Students\" JOIN \"NATQuest\".\"OwnedArtifactStudent\" " +
                $"ON \"NATQuest\".\"Students\".\"ID\" = \"NATQuest\".\"OwnedArtifactStudent\".\"StudentID\"" +
                $"JOIN \"NATQuest\".\"ArtifactStatus\" ON \"NATQuest\".\"OwnedArtifactStudent\".\"ArtifactStatusID\" = \"NATQuest\".\"ArtifactStatus\".\"ID\"" +
                $"JOIN \"NATQuest\".\"Artifacts\" ON \"NATQuest\".\"OwnedArtifactStudent\".\"ArtifactID\" = \"NATQuest\".\"Artifacts\".\"ID\" WHERE \"NATQuest\".\"OwnedArtifactStudent\".\"ArtifactStatusID\" = {statusArtifact} AND \"NATQuest\".\"Students\".\"ID\" ={id} ";
            using var command = new NpgsqlCommand(query, connection);
            var reader = command.ExecuteReader();

            var allRecords = new List<Artifact>();
            while (reader.Read())
            {
                allRecords.Add(ProvideOneRecord(reader));
            };
            return allRecords;

        }
        public List<Artifact> FetchAllGroupArtifacts(int id, int statusArtifact)
        {
            using NpgsqlConnection connection = OpenConnectionToDB();
            var query = $"SELECT \"NATQuest\".\"Artifacts\".\"ID\",\"NATQuest\".\"OwnedArtifactGroup\".\"ArtifactStatusID\", \"NATQuest\".\"Artifacts\".\"Name\", \"NATQuest\".\"Artifacts\".\"Cost\", \"NATQuest\".\"Artifacts\".\"Description\" FROM \"NATQuest\".\"Groups\" JOIN \"NATQuest\".\"OwnedArtifactGroup\" " +
                $"ON \"NATQuest\".\"Groups\".\"ID\" = \"NATQuest\".\"OwnedArtifactGroup\".\"GroupID\"" +
                $"JOIN \"NATQuest\".\"ArtifactStatus\" ON \"NATQuest\".\"OwnedArtifactGroup\".\"ArtifactStatusID\" = \"NATQuest\".\"ArtifactStatus\".\"ID\"" +
                $"JOIN \"NATQuest\".\"Artifacts\" ON \"NATQuest\".\"OwnedArtifactGroup\".\"ArtifactID\" = \"NATQuest\".\"Artifacts\".\"ID\" WHERE \"NATQuest\".\"OwnedArtifactGroup\".\"ArtifactStatusID\" = {statusArtifact} AND \"NATQuest\".\"Groups\".\"ID\" ={id} ";
            using var command = new NpgsqlCommand(query, connection);
            var reader = command.ExecuteReader();

            var allRecords = new List<Artifact>();
            while (reader.Read())
            {
                allRecords.Add(ProvideOneRecord(reader));
            };
            return allRecords;

        }

    }
}
