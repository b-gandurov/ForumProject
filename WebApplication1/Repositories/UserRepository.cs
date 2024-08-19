using ForumProject.Data;
using ForumProject.Models;
using ForumProject.Models.QueryParameters;
using ForumProject.Repositories.Contracts;
using ForumProject.Resources;
using ForumProject.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ForumProject.Repositories
{
    /// <summary>
    /// Repository for managing User-related operations.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="dbContext">Database context for accessing User data.</param>
        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets a User by their ID.
        /// </summary>
        /// <param name="id">The ID of the User to retrieve.</param>
        /// <returns>The User entity.</returns>
        /// <exception cref="EntityNotFoundException">Thrown when the User is not found.</exception>
        public User GetUserById(int id)
        {
            return GetUserEntity(id);
        }

        /// <summary>
        /// Gets a User by their username.
        /// </summary>
        /// <param name="username">The username of the User to retrieve.</param>
        /// <returns>The User entity.</returns>
        /// <exception cref="EntityNotFoundException">Thrown when the User is not found.</exception>
        public User GetUserByUsername(string username)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == username && u.DeletedAt == null);

            if (user == null)
            {
                throw new EntityNotFoundException(string.Format(ErrorMessages.ExceptionMessageUserWithUsernameNotExist, username));
            }

            return user;
        }

        /// <summary>
        /// Checks if a User exists by their username.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <returns>True if the User exists; otherwise, false.</returns>
        public bool UserExists(string username)
        {
            //this will check deleted users as well.
            //This way it will prevent users from being created with username of a User that was already deleted.
            //For optional account restore.(TODO)
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);

            return user != null;
        }



        /// <summary>
        /// Checks if an email is already registered.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>True if the email exists; otherwise, false.</returns>
        public bool EmailExists(string email)
        {
            if (email == null)
            {
                return false;
            }
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);

            return user != null;
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>List of all users.</returns>
        public List<User> GetAllUsers()
        {
            return _dbContext.Users.Where(u => u.DeletedAt == null).ToList();
        }

        /// <summary>
        /// Searches users by specified parameters.
        /// </summary>
        /// <param name="parameters">The query parameters for searching users.</param>
        /// <returns>List of users that match the search criteria.</returns>
        public List<User> SearchUsersBy(UserQueryParameters parameters)
        {
            IQueryable<User> query = _dbContext.Users.Where(u =>
                u.DeletedAt == null);

            if (!string.IsNullOrEmpty(parameters.Username))
            {
                query = query.Where(u => u.Username.Contains(parameters.Username));
            }

            if (!string.IsNullOrEmpty(parameters.Email))
            {
                query = query.Where(u => u.Email.Contains(parameters.Email));
            }

            return query.ToList();
        }

        /// <summary>
        /// Registers a new User.
        /// </summary>
        /// <param name="user">The User entity to register.</param>
        /// <exception cref="Exception">Thrown when the email already exists.</exception>
        public void RegisterUser(User user)
        {
            if (EmailExists(user.Email))
            {
                throw new DuplicateEntityException(string.Format(ErrorMessages.EmailTaken,user.Email));
            };
            if (UserExists(user.Username))
            {
                throw new DuplicateEntityException(string.Format(ErrorMessages.UsernameTaken, user.Username));
            };
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Updates an existing User.
        /// </summary>
        /// <param name="user">The User entity with updated details.</param>
        /// <exception cref="ArgumentNullException">Thrown when the User does not exist.</exception>
        public void UpdateUser(User user)
        {
            var userToUpdate = GetUserEntity(user.Id);
            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.Email = user.Email;
            userToUpdate.PhoneNumber = user.PhoneNumber;

            if (!string.IsNullOrEmpty(user.Password))
            {
                userToUpdate.Password = user.Password;
            }

            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Deletes a User by marking them as deleted.
        /// </summary>
        /// <param name="id">The ID of the User to delete.</param>
        public void DeleteUser(int id)
        {
            var user = GetUserEntity(id);
            //_dbContext.Remove(user);
            user.DeletedAt = DateTime.Now;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Blocks a User by their ID.
        /// </summary>
        /// <param name="id">The ID of the User to block.</param>
        public void BlockUser(int id)
        {
            var user = GetUserEntity(id);
            user.IsBlocked = true;

            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Unblocks a User by their ID.
        /// </summary>
        /// <param name="id">The ID of the User to unblock.</param>
        public void UnblockUser(int id)
        {
            var user = GetUserEntity(id);
            user.IsBlocked = false;

            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Promotes a User to admin by their ID.
        /// </summary>
        /// <param name="id">The ID of the User to promote.</param>
        /// <exception cref="ArgumentNullException">Thrown when the User does not exist or is already an admin.</exception>
        public void PromoteUserToAdmin(int id)
        {
            var userToPromote = GetUserEntity(id);

            if (userToPromote.IsAdmin)
            {
                throw new ArgumentNullException(ErrorMessages.ExceptionMessageAlreadyAdmin);
            }

            userToPromote.IsAdmin = true;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Demotes a User from admin by their ID.
        /// </summary>
        /// <param name="id">The ID of the User to demote.</param>
        /// <exception cref="EntityNotFoundException">Thrown when the User is not found.</exception>
        /// <exception cref="BadRequestException">Thrown when the User is not an admin.</exception>
        public void DemoteUserFromAdmin(int id)
        {
            var userToDemote = GetUserEntity(id);
            if (!userToDemote.IsAdmin)
            {
                throw new BadRequestException(ErrorMessages.ExceptionMessageAlreadyNotAdmin);
            }

            userToDemote.IsAdmin = false;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Retrieves the User entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the User entity to retrieve.</param>
        /// <returns>The User entity.</returns>
        /// <exception cref="EntityNotFoundException">Thrown when the User is not found.</exception>
        private User GetUserEntity(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == id && u.DeletedAt == null);

            if (user == null)
            {
                throw new EntityNotFoundException(string.Format(ErrorMessages.ExceptionMessageUserWithIdNotExist, id));
            }

            return user;
        }

        public void UploadProfilePicture(string username, string pictureUrl)
        {
            var user = GetUserByUsername(username);

            user.ProfilePictureUrl = pictureUrl;
            _dbContext.SaveChanges();
        }
    }
}
