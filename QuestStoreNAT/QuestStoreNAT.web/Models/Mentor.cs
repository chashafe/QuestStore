using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuestStoreNAT.web.Models
{
    public class Mentor : IUser
    {
        public int Id { get; set; }

        public List<Classroom> MentorClassrooms { get; set; }
        public List<Group> MentorGroups { get; set; }
        public List<Student> MentorStudents { get; set; }

        [Required]
        public int CredentialID { get; set; }

        [Display(Name = "First name: ")]
        [Required(ErrorMessage = "First name required")]
        [StringLength(20, ErrorMessage = "2 to 20 characters.", MinimumLength = 2)]
        public string FirstName { get; set; }

        [Display(Name = "Last name: ")]
        [Required(ErrorMessage = "Last name required")]
        [StringLength(20, ErrorMessage = "2 to 20 characters.", MinimumLength = 2)]
        public string LastName { get; set; }


        [Display(Name = "A short Bio: ")]
        [StringLength(255, ErrorMessage = "2 to 255 characters.", MinimumLength = 2)]
        public string Bio { get; set; }
    }
}
