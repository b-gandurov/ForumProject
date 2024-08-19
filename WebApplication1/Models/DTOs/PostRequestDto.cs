using ForumProject.Models.Enums;
using ForumProject.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace ForumProject.Models.DTOs
{
    public class PostRequestDto
    {

        //public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The {0} field is required and must not be an empty string.")]
        [StringLength(64, MinimumLength = 16, ErrorMessage = "The {0} field must be between {2} and {1} characters.")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The {0} field is required and must not be an empty string.")]
        [StringLength(8192, MinimumLength = 32, ErrorMessage = "The {0} field must be between {2} and {1} characters.")]
        public string Content { get; set; }

        public PostCategory Category { get; set; }

        public string? ImageUrl { get; set; }

     
        //[Required(AllowEmptyStrings = false, ErrorMessage = "The {0} field is required and must not be an empty string.")]
        //public int UserId { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public List<CommentRequestDto>? Comments { get; set; }
        //public List<int>? ReactionIds { get; set; }

    }
}
