using ForumProject.Models.Enums;
using ForumProject.Models.ViewModels;

namespace ForumProject.Models.DTOs
{
    public class PostResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; }

        public string? ImageUrl { get; set; }

        public PostCategory Category { get; set; }

        

        public List<int> ReactionIds { get; set; }
        public List<CommentResponseDto>? Comments { get; set; }
    }
}
