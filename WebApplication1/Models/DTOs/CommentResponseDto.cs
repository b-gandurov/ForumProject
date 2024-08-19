namespace ForumProject.Models.DTOs
{
    public class CommentResponseDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int PostId { get; set; }
        public int? ParentCommentId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }

        public ICollection<ReactionResponseDto>? Reactions { get; set; }
        public ICollection<CommentResponseDto>? Replies { get; set; }
    }
}
