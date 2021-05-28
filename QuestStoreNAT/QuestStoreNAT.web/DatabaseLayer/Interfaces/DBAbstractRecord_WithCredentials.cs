using Npgsql;

namespace QuestStoreNAT.web.DatabaseLayer
{
    public abstract class DBAbstractRecord_WithCredentials<T> : DBAbstractRecord<T>
    {
        public virtual T FindOneRecordByCredentialId(int CredentialsId)
        {
            using NpgsqlConnection connection = OpenConnectionToDB();
            var query = $"SELECT * FROM \"NATQuest\".\"{DBTableName}\" WHERE \"NATQuest\".\"{DBTableName}\".\"Credential_ID\" = '{CredentialsId}' LIMIT 1;";
            using var command = new NpgsqlCommand(query, connection);
            var reader = command.ExecuteReader();

            var oneRecord = default(T);
            while (reader.Read())
            {
                oneRecord = ProvideOneRecord(reader);
            };
            return oneRecord;
        }
    }
}
