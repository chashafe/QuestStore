using System;
using System.ComponentModel.DataAnnotations;

namespace QuestStoreNAT.web.Models
{
    public class StudentAcceptance
    {
        public int ID { get; set; }
        public int studentID { get; set; }
        public int artifactID { get; set; }

        [Display(Name = "Acceptance:")]
        [Required(ErrorMessage = "Acceptance required")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer")]
        [RegularExpression(@"^[1-2]*$", ErrorMessage = "Please enter whole number")]
        public int acceptance { get; set; }

        public int groupID { get; set; }

        public string artifactName { get; set; }

        public int currentAmountOfAcceptance { get; set; }
        public int costArtifact { get; set; }

    }


}
