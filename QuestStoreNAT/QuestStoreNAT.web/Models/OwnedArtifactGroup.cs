using System.ComponentModel.DataAnnotations;

namespace QuestStoreNAT.web.Models
{
    public class OwnedArtifactGroup
    {
        public int Id { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public int ArtifactId { get; set; }

        [Required]
        public CompletionStatus CompletionStatus { get; set; }
    }
}
