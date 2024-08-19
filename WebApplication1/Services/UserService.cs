using ForumProject.Models;
using ForumProject.Models.QueryParameters;
using ForumProject.Repositories.Contracts;
using ForumProject.Services.Contracts;
using ForumProject.Exceptions;

namespace ForumProject.Services
{
    /// <summary>
    /// Service for managing User-related operations.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userRepository">Repository for User operations.</param>
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Registers a new User.
        /// </summary>
        /// <param name="user">The User entity to register.</param>
        /// <exception cref="ArgumentException">Thrown when User details are invalid.</exception>
        public void RegisterUser(User user)
        {
            _userRepository.RegisterUser(user);
        }

        /// <summary>
        /// Blocks a User by their ID.
        /// </summary>
        /// <param name="id">The ID of the User to block.</param>
        /// <exception cref="ArgumentNullException">Thrown when the User does not exist.</exception>
        public void BlockUser(int id)
        {
            _userRepository.BlockUser(id);
        }

        /// <summary>
        /// Deletes a User by their ID.
        /// </summary>
        /// <param name="id">The ID of the User to delete.</param>
        /// <exception cref="ArgumentNullException">Thrown when the User does not exist.</exception>
        public void DeleteUser(int id)
        {
            _userRepository.DeleteUser(id);
        }

        /// <summary>
        /// Searches users by specified parameters.
        /// </summary>
        /// <param name="parameters">The query parameters for searching users.</param>
        /// <returns>List of users that match the search criteria.</returns>
        public IEnumerable<User> SearchUsersBy(UserQueryParameters parameters)
        {
            return _userRepository.SearchUsersBy(parameters);
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>List of all users.</returns>
        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        /// <summary>
        /// Gets a User by their ID.
        /// </summary>
        /// <param name="id">The ID of the User to retrieve.</param>
        /// <returns>The User entity.</returns>
        /// <exception cref="EntityNotFoundException">Thrown when the ID is invalid.</exception>
        public User GetUserById(int id)
        {
            return _userRepository.GetUserById(id);
        }

        /// <summary>
        /// Gets a User by their username.
        /// </summary>
        /// <param name="username">The username of the User to retrieve.</param>
        /// <returns>The User entity.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the username is null or empty.</exception>
        public User GetUserByUsername(string username)
        {
            return _userRepository.GetUserByUsername(username);
        }

        /// <summary>
        /// Unblocks a User by their ID.
        /// </summary>
        /// <param name="id">The ID of the User to unblock.</param>
        /// <exception cref="ArgumentNullException">Thrown when the User does not exist.</exception>
        public void UnblockUser(int id)
        {
            _userRepository.UnblockUser(id);
        }

        /// <summary>
        /// Updates an existing User.
        /// </summary>
        /// <param name="user">The User entity with updated details.</param>
        /// <exception cref="ArgumentNullException">Thrown when the User is null.</exception>
        public void UpdateUser(User user)
        {
            _userRepository.UpdateUser(user);
        }

        /// <summary>
        /// Promotes a User to admin by their ID.
        /// </summary>
        /// <param name="id">The ID of the User to promote.</param>
        /// <exception cref="ArgumentException">Thrown when the ID is invalid.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the User does not exist.</exception>
        public void PromoteUserToAdmin(int id)
        {
            _userRepository.PromoteUserToAdmin(id);
        }

        /// <summary>
        /// Demotes a User from admin by their ID.
        /// </summary>
        /// <param name="id">The ID of the User to demote.</param>
        /// <exception cref="ArgumentException">Thrown when the ID is invalid.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the User does not exist.</exception>
        public void DemoteUserFromAdmin(int id)
        {
            _userRepository.DemoteUserFromAdmin(id);
        }
    }
}
