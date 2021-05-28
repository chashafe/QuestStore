using QuestStoreNAT.web.DatabaseLayer;
using QuestStoreNAT.web.Models;
using System.Linq;

namespace QuestStoreNAT.web.Services
{
    public class MentorDetails
    {
        private readonly ClassEnrolmentDAO _classEnrolmentDAO;

        public MentorDetails(ClassEnrolmentDAO classEnrolmentDAO)
        {
            _classEnrolmentDAO = classEnrolmentDAO;
        }

        public Mentor GetMentorClassrooms(Mentor mentor)
        {
            var mentorClassrooms = _classEnrolmentDAO.FetchAllRecordsJoin().Where(ce => ce.MentorCE.Id == mentor.Id).Select(ce => ce.ClassroomCE).ToList();
            mentor.MentorClassrooms = mentorClassrooms;
            return mentor;
        }
    }
}
