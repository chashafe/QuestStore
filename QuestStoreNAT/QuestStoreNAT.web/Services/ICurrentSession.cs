using QuestStoreNAT.web.Models;

namespace QuestStoreNAT.web.Services
{
    public interface ICurrentSession
    {
        public IUser LoggedUser { get; set; }
        public Role LoggedUserRole { get; set; }
    }
}
