namespace ForumProject.Helpers
{
    public interface IAuthManager
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }

    
}

