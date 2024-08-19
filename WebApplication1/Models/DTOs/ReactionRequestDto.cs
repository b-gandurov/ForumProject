using ForumProject.Models.Enums;

namespace ForumProject.Models.DTOs
{
    public class ReactionRequestDto
    {
        //TODO Maria

        public int Id { get; set; }
        public ReactionType ReactionType { get; set; }

        public int? PostId { get; set; }
        public int? CommentId { get; set; }
        public int UserId { get; set; }
    }
}
