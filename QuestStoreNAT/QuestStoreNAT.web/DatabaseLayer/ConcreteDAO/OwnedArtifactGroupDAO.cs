using System;
using Npgsql;
using QuestStoreNAT.web.Models;

namespace QuestStoreNAT.web.DatabaseLayer
{
    public class OwnedArtifactGroupDAO : DBAbstractRecord<OwnedArtifactGroup>
    {
        public override string DBTableName { get; set; } = "OwnedArtifactGroup";

        private enum OwnedArtifactGroupEnum
        {
            Id, GroupId, ArtifactId, CompletionStatus,
        }

        public override OwnedArtifactGroup ProvideOneRecord(NpgsqlDataReader reader)
        {
            var ownedArtifactGroup = new OwnedArtifactGroup();

            ownedArtifactGroup.Id = reader.GetInt32((int)OwnedArtifactGroupEnum.Id);
            ownedArtifactGroup.GroupId = reader.GetInt32((int)OwnedArtifactGroupEnum.GroupId);
            ownedArtifactGroup.ArtifactId = reader.GetInt32((int)OwnedArtifactGroupEnum.ArtifactId);
            ownedArtifactGroup.CompletionStatus = (CompletionStatus)reader.GetInt32((int)OwnedArtifactGroupEnum.CompletionStatus);
            return ownedArtifactGroup;
        }

        public override string ProvideQueryStringToAdd(OwnedArtifactGroup OwnedArtifactGroupToAdd)
        {
            var query = $"INSERT INTO \"NATQuest\".\"{DBTableName}\" (\"GroupID\", \"ArtifactID\", \"ArtifactStatusID\")" +
                        $"VALUES({OwnedArtifactGroupToAdd.GroupId}, " +
                               $"'{OwnedArtifactGroupToAdd.ArtifactId}', " +
                               $"'{(int)OwnedArtifactGroupToAdd.CompletionStatus}');";
            return query;
        }

        public override string ProvideQueryStringToUpdate(OwnedArtifactGroup OwnedArtifactGroupToUpdate)
        {
            var query = $"UPDATE \"NATQuest\".\"{DBTableName}\" " +
                        $"SET \"GroupID\" = {OwnedArtifactGroupToUpdate.GroupId}, " +
                            $"\"ArtifactID\" = '{OwnedArtifactGroupToUpdate.ArtifactId}', " +
                            $"\"ArtifactStatusID\" = '{(int)OwnedArtifactGroupToUpdate.CompletionStatus}' " +
                        $"WHERE \"NATQuest\".\"{DBTableName}\".\"ID\" = {OwnedArtifactGroupToUpdate.Id};";
            return query;
        }

        public OwnedArtifactGroup FindOneRecordBy(int id, int groupID, CompletionStatus completiotStatus)
        {
            using NpgsqlConnection connection = OpenConnectionToDB();
            var query = $"SELECT * FROM \"NATQuest\".\"{DBTableName}\" WHERE \"ArtifactID\" = '{id}' AND \"GroupID\" = '{groupID}' AND \"ArtifactStatusID\" = {(int)completiotStatus} LIMIT 1;";
            using var command = new NpgsqlCommand(query, connection);
            var reader = command.ExecuteReader();

            var oneRecord = default(OwnedArtifactGroup);
            while (reader.Read())
            {
                oneRecord = ProvideOneRecord(reader);
            };
            return oneRecord;
        }

    }
}
