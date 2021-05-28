using System.ComponentModel.DataAnnotations;

namespace QuestStoreNAT.web.Models
{
    public class ClassEnrollment
    {
        public int Id { get; set; }

        //[Required]
        //public int ClassroomId { get; set; }

        //[Required]
        //public int MentorID { get; set; }
        public virtual Mentor MentorCE {get; set;}
        public virtual Classroom ClassroomCE { get; set; }
    }
}
