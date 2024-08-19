namespace ForumProject.Models.QueryParameters
{
    public class UserQueryParameters
    {
        //TODO Maria

        public string? Username { get; set; } // Filters users by username
        public string? Email { get; set; } // Filters users by email
        public bool IsAdmin { get; set; } // Filters users admin status
        public bool IsBlocked { get; set; } //Filters users by blocked status
        
    }
}
