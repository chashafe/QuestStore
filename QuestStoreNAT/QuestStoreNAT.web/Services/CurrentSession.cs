using QuestStoreNAT.web.Models;

namespace QuestStoreNAT.web.Services
{
    public class CurrentSession : ICurrentSession
    {
        public IUser LoggedUser { get; set; }
        public Role LoggedUserRole { get; set; }
    }
}
