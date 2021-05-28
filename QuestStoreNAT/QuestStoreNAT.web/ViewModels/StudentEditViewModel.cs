using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuestStoreNAT.web.Models;

namespace QuestStoreNAT.web.ViewModels
{
    public class StudentEditViewModel
    {
        public int Id { get; set; }

        [Required]
        public int ClassID { get; set; }

        public int GroupID { get; set; }

        [Required]
        public virtual int CredentialID { get; set; }

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
        public IEnumerable<Classroom> Classrooms { get; set; }
        public IEnumerable<Group> Groups { get; set; }
        public IEnumerable<OwnedQuestStudent> OwnedStudentQuests { get; set; }
    }
}
