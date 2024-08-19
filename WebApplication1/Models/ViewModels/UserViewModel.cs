namespace ForumProject.Models.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IFormFile? ProfilePicture { get; set; }

        public string? ProfilePictureURL { get; set; }
    }

}
