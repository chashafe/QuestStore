using QuestStoreNAT.web.Models;

namespace QuestStoreNAT.web.Services
{
    public interface IUserFinderService
    {
        IUser RetrieveUser(Role role, int credentialId);
    }
}
