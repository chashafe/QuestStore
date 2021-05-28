using System.ComponentModel.DataAnnotations;

namespace QuestStoreNAT.web.Models
{
    public class Artifact
    {
        public int Id { get; set; }

        [Display(Name = "Type:")]
        [Required(ErrorMessage = "Type required")]
        public TypeClassification Type { get; set; }

        [Display(Name = "Name:")]
        [Required(ErrorMessage = "Name required")]
        [StringLength(20, ErrorMessage = "1 to 20 characters.", MinimumLength = 1)]
        public string Name { get; set; }

        [Display(Name = "Cost:")]
        [Required(ErrorMessage = "Cost required")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Please enter whole number")]
        public int Cost { get; set; }

        [Display(Name = "Description:")]
        [Required(ErrorMessage = "Description required")]
        [StringLength(255, ErrorMessage = "1 to 255 characters.", MinimumLength = 1)]
        public string Description { get; set; }
    }
}
