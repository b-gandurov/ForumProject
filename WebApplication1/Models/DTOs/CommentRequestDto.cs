using System.ComponentModel.DataAnnotations;

namespace ForumProject.Models.DTOs
{
    public class CommentRequestDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "The {0} field is required and must not be an empty string.")]
        [StringLength(8192, MinimumLength = 10, ErrorMessage = "The {0} field must be between {2} and {1} characters.")]
        public string Content { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public int PostId { get; set; }

        public int? ParentCommentId { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        public int UserId { get; set; }

        public List<Reaction>? Reactions { get; set; }
    }
}
