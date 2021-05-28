using System;
using QuestStoreNAT.web.Models;
using Npgsql;

namespace QuestStoreNAT.web.DatabaseLayer
{
    public class CredentialsDAO : DBAbstractRecord_WithIdReturning<Credentials>
    {
        public override string DBTableName { get; set; } = "Credentials";

        private enum CredentialsEnum
        {
            Id, Role, Email, Password, Salt
        }

        #region standardDAOimplementation
        public override Credentials ProvideOneRecord(NpgsqlDataReader reader)
        {
            var credentials = new Credentials();
            credentials.Id = reader.GetInt32((int)CredentialsEnum.Id);
            credentials.Role = (Role)reader.GetInt32((int)CredentialsEnum.Role);
            credentials.Email = reader.GetString((int)CredentialsEnum.Email);
            credentials.Password = reader.GetString((int)CredentialsEnum.Password);
            credentials.SALT = reader.GetString((int)CredentialsEnum.Salt);
            return credentials;
        }

        public override string ProvideQueryStringToUpdate(Credentials recordToUpdate)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region IdReturningDAOimplementation
        public override string ProvideQueryStringToAdd( Credentials credentialToAdd )
        {
            var query = $"INSERT INTO \"NATQuest\".\"{DBTableName}\" (\"Role\", \"Email\", \"Password\", \"Salt\")" +
                        $"VALUES({(int)credentialToAdd.Role}, " +
                               $"'{(string)credentialToAdd.Email}', " +
                               $"'{(string)credentialToAdd.Password}', " +
                               $"'{(string)credentialToAdd.SALT}') RETURNING \"ID\";";
            return query;
        }

        public override int AddRecordReturningID(Credentials newCredential)
        {
            using NpgsqlConnection connection = OpenConnectionToDB();
            string query = ProvideQueryStringToAdd(newCredential);
            return ExecuteScalar(connection, query);
        }
        #endregion
    }
}
