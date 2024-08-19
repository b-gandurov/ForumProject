using System.Text;
using ForumProject.Exceptions;
using ForumProject.Resources;
using ForumProject.Services.Contracts;

namespace ForumProject.Helpers
{
    public class AuthManager : IAuthManager
    {
        private readonly IUserService _userService;
        public AuthManager(IUserService userService)
        {
            _userService = userService;
        }
        public string HashPassword(string password)
        {
            string encodedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));

            return encodedPassword;
        }

        public bool VerifyPassword(string username, string passwordToCheck)
        {
            var user = _userService.GetUserByUsername(username);
            if (user == null)
            {
                throw new EntityNotFoundException(string.Format(ErrorMessages.UserNotFound, username));
            }
            var hashedUserPassword = HashPassword(user.Password);
            string encodedPassword = HashPassword(passwordToCheck);

            return hashedUserPassword == encodedPassword;
        }
    }
}
