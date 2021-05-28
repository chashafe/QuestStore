using System;
namespace QuestStoreNAT.web.Models
{
    public class GroupTransaction
    {
        public int ID { get; set; }
        public int artifactID { get; set; }
        public int groupID { get; set; }
        public int numberOfStudents { get; set; }
        public int numberOfAcceptance { get; set; }
    }
}
