using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using QuestStoreNAT.web.Models;

namespace QuestStoreNAT.web.DatabaseLayer
{
    public class ClassroomDAO : DBAbstractRecord<Classroom>
    {
        public override string DBTableName { get; set; } = "Classes";

        public override Classroom ProvideOneRecord( NpgsqlDataReader reader )
        {
            var classroom = new Classroom
            {
                Id = reader.GetInt32(0) ,
                Name = reader.GetString(1)
            };
            return classroom;
        }

        public override string ProvideQueryStringToAdd( Classroom classroomToAdd )
        {
            var query = $"INSERT INTO \"NATQuest\".\"{DBTableName}\" (\"Name\")" +
                 $"VALUES('{classroomToAdd.Name}');";
            return query;
        }

        public override string ProvideQueryStringToUpdate( Classroom clasroomToUpdate )
        {
            var query = $"UPDATE \"NATQuest\".\"{DBTableName}\" SET \"Name\" = '{clasroomToUpdate.Name}'" +
                    $"WHERE \"ID\" = {clasroomToUpdate.Id};";
            return query;
        }
    }
}
