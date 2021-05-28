using System.ComponentModel.DataAnnotations;

namespace QuestStoreNAT.web.Models
{
    public class OwnedQuestGroup
    {
        public int Id { get; set; }

        [Required]
        public int QuestId { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public CompletionStatus CompletionStatus { get; set; }
    }
}
