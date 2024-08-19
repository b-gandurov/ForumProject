using System.ComponentModel.DataAnnotations;

namespace ForumProject.Models.DTOs
{
    public class UserRequestDto
    {
        //TODO Maria
        [Required(AllowEmptyStrings = false, ErrorMessage = "The {0} field is required and must not be an empty string.")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The {0} field is required and must not be an empty string.")]
        public string Password { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "The {0} field is required and must not be an empty string.")]
        public string Email { get; set; }

        [StringLength(32, MinimumLength = 4, ErrorMessage = "The {0} field must be between {2} and {1} characters.")]
        public string FirstName { get; set; }

        [StringLength(32, MinimumLength = 4, ErrorMessage = "The {0} field must be between {2} and {1} characters.")]
        public string LastName { get; set; }
    }
}
