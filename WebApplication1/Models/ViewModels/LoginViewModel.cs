using System.ComponentModel.DataAnnotations;

namespace ForumProject.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(32, MinimumLength = 4, ErrorMessage = "Title must be between 4 and 32 characters.")]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
