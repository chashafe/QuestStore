using Npgsql;
using System;

namespace QuestStoreNAT.web.DatabaseLayer
{
    public abstract class DBAbstractRecord_WithIdReturning<T> : DBAbstractRecord<T>
    {
        public virtual int AddRecordReturningID( T recordToAdd )
        {
            using NpgsqlConnection connection = OpenConnectionToDB();
            string query = ProvideQueryStringToAdd(recordToAdd);
            return ExecuteScalar(connection , query);
        }

        protected int ExecuteScalar( NpgsqlConnection connection , string query )
        {
            using var command = new NpgsqlCommand(query , connection);
            command.Prepare();
            return Convert.ToInt32(command.ExecuteScalar());
        }
    }
}
