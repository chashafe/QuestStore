using Npgsql;
using System.Collections.Generic;

namespace QuestStoreNAT.web.DatabaseLayer
{
    public abstract class DBAbstractRecord<T> : IDB_GenericInterface<T>
    {
        public abstract string DBTableName { get; set; }
        public abstract T ProvideOneRecord(NpgsqlDataReader reader);
        public abstract string ProvideQueryStringToAdd(T recordToAdd);
        public abstract string ProvideQueryStringToUpdate(T recordToUpdate);

        public virtual T FindOneRecordBy(string email)
        {
            using NpgsqlConnection connection = OpenConnectionToDB();
            var query = $"SELECT * FROM \"NATQuest\".\"{DBTableName}\" WHERE \"NATQuest\".\"{DBTableName}\".\"Email\" = '{email}' LIMIT 1;";
            using var command = new NpgsqlCommand(query, connection);
            var reader = command.ExecuteReader();

            var oneRecord = default(T);
            while (reader.Read())
            {
                oneRecord = ProvideOneRecord(reader);
            };
            return oneRecord;
        }

        public virtual T FindOneRecordBy(int id)
        {
            using NpgsqlConnection connection = OpenConnectionToDB();
            var query = $"SELECT * FROM \"NATQuest\".\"{DBTableName}\" WHERE \"NATQuest\".\"{DBTableName}\".\"ID\" = '{id}' LIMIT 1;";
            using var command = new NpgsqlCommand(query, connection);
            var reader = command.ExecuteReader();

            var oneRecord = default(T);
            while (reader.Read())
            {
                oneRecord = ProvideOneRecord(reader);
            };
            return oneRecord;
        }

        public List<T> FetchAllRecords()
        {
            using NpgsqlConnection connection = OpenConnectionToDB();
            var query = $"SELECT * FROM \"NATQuest\".\"{DBTableName}\"";
            using var command = new NpgsqlCommand(query, connection);
            var reader = command.ExecuteReader();

            var allRecords = new List<T>();
            while (reader.Read())
            {
                allRecords.Add(ProvideOneRecord(reader));
            };
            return allRecords;
        }

        public virtual void AddRecord(T recordToAdd)
        {
            using NpgsqlConnection connection = OpenConnectionToDB();
            string query = ProvideQueryStringToAdd(recordToAdd);
            ExecuteQuery(connection, query);
        }

        public virtual void UpdateRecord(T recordToUpdate)
        {
            using NpgsqlConnection connection = OpenConnectionToDB();
            string query = ProvideQueryStringToUpdate(recordToUpdate);
            ExecuteQuery(connection, query);
        }

        public virtual void DeleteRecord(int id)
        {
            using NpgsqlConnection connection = OpenConnectionToDB();
            string query = $"DELETE FROM \"NATQuest\".\"{DBTableName}\" WHERE \"NATQuest\".\"{DBTableName}\".\"ID\" = {id}";
            ExecuteQuery(connection, query);
        }

        protected NpgsqlConnection OpenConnectionToDB()
        {
            var connection = new NpgsqlConnection(ConnectDB.GetConnectionString());
            connection.Open();
            return connection;
        }

        protected void ExecuteQuery(NpgsqlConnection connection, string query)
        {
            using var command = new NpgsqlCommand(query, connection);
            command.Prepare();
            command.ExecuteNonQuery();
        }
    }
}
