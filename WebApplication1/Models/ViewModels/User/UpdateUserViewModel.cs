namespace ForumProject.Models.ViewModels.User
{
    public class UpdateUserViewModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }

        public string? NewPassword { get; set; }
        public string? NewPasswordConfirmation { get; set; }

        public bool IsAdmin { get; set; }
        public int UserId { get; set; }
    }
}
