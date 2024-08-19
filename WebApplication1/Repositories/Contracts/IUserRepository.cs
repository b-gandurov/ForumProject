using ForumProject.Models;
using ForumProject.Models.QueryParameters;

namespace ForumProject.Repositories.Contracts
{
    public interface IUserRepository
    {
        public User GetUserById(int id);

        public User GetUserByUsername(string username);

        public List<User> GetAllUsers();

        public List<User> SearchUsersBy(UserQueryParameters parameters);

        public void RegisterUser(User user);

        public void UpdateUser(User user);

        public void DeleteUser(int id);

        public void BlockUser(int id);

        public void UnblockUser(int id);

        public void PromoteUserToAdmin(int id);

        public void DemoteUserFromAdmin(int id);

        public bool UserExists(string username);
        public bool EmailExists(string username);
        public void UploadProfilePicture(string username, string pictureUrl);
    }
}
