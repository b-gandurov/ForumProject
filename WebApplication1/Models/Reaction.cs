using ForumProject.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForumProject.Models
{
    public class Reaction
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public ReactionType ReactionType { get; set; }

        [ForeignKey("ReactionTarget")]
        public int ReactionTargetId { get; set; }
        public ReactionTarget ReactionTarget { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
