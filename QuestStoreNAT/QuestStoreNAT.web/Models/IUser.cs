namespace QuestStoreNAT.web.Models
{
    public interface IUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CredentialID { get; set; }
    }
}
