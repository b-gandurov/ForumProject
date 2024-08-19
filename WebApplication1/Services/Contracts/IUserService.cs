using ForumProject.Models;
using ForumProject.Models.QueryParameters;

namespace ForumProject.Services.Contracts
{
    public interface IUserService
    {
        public void RegisterUser(User user);

        public void BlockUser(int id);

        public void DeleteUser(int id);

        public IEnumerable<User> SearchUsersBy(UserQueryParameters parameters);

        public IEnumerable<User> GetAllUsers();

        public User GetUserById(int id);

        public User GetUserByUsername(string username);

        public void UnblockUser(int id);

        public void UpdateUser(User user);

        public void PromoteUserToAdmin(int id);

        public void DemoteUserFromAdmin(int id);
    }

}
