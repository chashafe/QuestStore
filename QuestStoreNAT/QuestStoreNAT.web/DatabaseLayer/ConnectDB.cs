using Microsoft.Extensions.Configuration;

namespace QuestStoreNAT.web.DatabaseLayer
{
    public static class ConnectDB
    {
        public static IConfiguration Configuration;

        public static string GetConnectionString()
        {
            return Configuration.GetConnectionString("ElephantSQL");
        }
    }
}
