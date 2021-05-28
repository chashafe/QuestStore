using System.ComponentModel.DataAnnotations;

namespace QuestStoreNAT.web.Models
{
    public class ContactForm
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Subject { get; set; }

        public string Message { get; set; }
    }
}
