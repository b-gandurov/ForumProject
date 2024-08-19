using ForumProject.Models.Enums;

namespace ForumProject.Models.DTOs
{
    public class ReactionResponseDto
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public ReactionType ReactionType { get; set; }

        public int? PostId { get; set; }

        public int? CommentId { get; set; }
        public int UserId { get; set; }
    }
}
