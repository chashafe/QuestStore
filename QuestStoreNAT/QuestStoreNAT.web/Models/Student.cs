using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuestStoreNAT.web.Models
{
    public class Student : IUser
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Class name: ")]
        public int ClassID { get; set; }

        [Display(Name = "Group name: ")]
        public int GroupID { get; set; }

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

        [Required]
        public int Wallet { get; set; }

        [Required]
        public int OverallWalletLevel { get; set; }

        public List<OwnedQuestStudent> OwnedStudentQuests { get; set; }
        public List<OwnedQuestStudent> OwnedGroupQuests { get; set; }
        public List<Quest> StudentQuests { get; set; }
        public List<Artifact> StudentArtifacts { get; set; }
        public List<Artifact> UsedStudentArtifacts { get; set; }
        public List<Artifact> GroupArtifacts { get; set; }
        public List<Artifact> UsedGroupArtifacts { get; set; }
        public string groupName { get; set; }
        public int level { get; set; }
    }
}
