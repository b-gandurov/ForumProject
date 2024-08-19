using System.ComponentModel.DataAnnotations;

namespace ForumProject.Models
{
    public class User
    {
        //TODO Maria

        //- Phonenumber (if admin)
        [Required]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public bool IsAdmin { get; set; } = false;
        public bool IsBlocked { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public List<Post>? Posts { get; set; }
        public List<Comment>? Comments { get; set; }

        public List<Reaction>? Reactions { get; set; }
        public string? PhoneNumber { get; set; }
        public string ProfilePictureUrl { get; set; } = "/images/default.jpg";

    }
}
