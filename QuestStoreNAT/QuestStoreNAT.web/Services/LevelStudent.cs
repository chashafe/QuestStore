using System;
namespace QuestStoreNAT.web.Services
{
    public class LevelStudent
    {
        public int levelStudent(double overWallet)
        {
            int levelStudent = (int)Math.Floor(overWallet / 100);
            return levelStudent;
        }
    }
}


