namespace ForumProject.Models.DTOs
{
    public class UserResponseDto
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsBlocked { get; set; }
        public List<PostResponseDto> Posts { get; set; }
        public List<CommentResponseDto> Comments { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
