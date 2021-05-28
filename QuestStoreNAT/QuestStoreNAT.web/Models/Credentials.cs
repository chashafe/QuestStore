using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QuestStoreNAT.web.Models
{
    public class Credentials
    {
        public int Id { get; set; }

        [Display(Name = "E-mail: ")]
        [Required(ErrorMessage = "Email required")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        [DataType(DataType.EmailAddress)]
        public virtual string Email { get; set; }


        [Display(Name = "Password: ")]
        [Required]
        [DataType(DataType.Password)]
        public virtual string Password { get; set; }

        [Required]
        public virtual string SALT { get; set; }

        [Display(Name = "Choose a role: ")]
        [Required]
        public Role Role { get; set; }
    }
}
