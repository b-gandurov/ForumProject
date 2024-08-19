using System.ComponentModel.DataAnnotations;

namespace ForumProject.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Username field is required.")]
        public string Username {  get; set; }

        [Required(ErrorMessage = "Password field is required.")]
        public string Password {  get; set; }

        [Required(ErrorMessage = "ConfirmPassword field is required.")]
        [Compare(nameof(Password), ErrorMessage = "Passwords don't match.")]
        public string ConfirmPassword {  get; set; }
    }
}
