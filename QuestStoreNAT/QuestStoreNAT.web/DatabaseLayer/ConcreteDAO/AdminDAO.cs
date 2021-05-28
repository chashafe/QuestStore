using Npgsql;
using QuestStoreNAT.web.Models;

namespace QuestStoreNAT.web.DatabaseLayer
{
    public class AdminDAO : DBAbstractRecord_WithCredentials<Admin>
    {
        public override string DBTableName { get; set; } = "Admins";
        private enum AdminsEnum
        {
            Id, FirstName, LastName
        }

        public override Admin ProvideOneRecord(NpgsqlDataReader reader)
        {
            var admin = new Admin();
            admin.Id = reader.GetInt32((int)AdminsEnum.Id);
            admin.FirstName = reader.GetString((int)AdminsEnum.FirstName);
            admin.LastName = reader.GetString((int)AdminsEnum.LastName);
            return admin;
        }

        public override string ProvideQueryStringToAdd(Admin adminToAdd)
        {
            throw new System.NotImplementedException();
        }

        public override string ProvideQueryStringToUpdate(Admin adminToUpdate)
        {
            throw new System.NotImplementedException();
        }
    }
}
