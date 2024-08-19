using ForumProject.Data;
using ForumProject.Exceptions;
using ForumProject.Models;
using ForumProject.Models.QueryParameters;
using ForumProject.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ForumProject.Tests.Repositories
{
    [TestClass]
    public class UserRepositoryTests
    {
        private Mock<ApplicationDbContext> _dbContextMock;
        private ApplicationDbContext _dbContext;
        private UserRepository _userRepository;
        private List<User> _users;

        [TestInitialize]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .EnableSensitiveDataLogging() // Enable sensitive data logging for better error messages
                .Options;

            _dbContext = new ApplicationDbContext(options);

            // Seed the database with test data including all required properties
            _dbContext.Users.AddRange(new List<User>
            {
                new User
                {
                    Id = 1,
                    Username = "testuser1",
                    Password = "Password1!", // Ensure this required field is included
                    Email = "test1@example.com",
                    FirstName = "First1",
                    LastName = "Last1"
                },
                new User
                {
                    Id = 2,
                    Username = "testuser2",
                    Password = "Password2!", // Ensure this required field is included
                    Email = "test2@example.com",
                    FirstName = "First2",
                    LastName = "Last2"
                }
            });
            _dbContext.SaveChanges();

            _userRepository = new UserRepository(_dbContext);
        }


        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [TestMethod]
        public void GetUserById_Should_ReturnUser_When_UserExists()
        {
            // Arrange
            var userId = 1;

            // Act
            var result = _userRepository.GetUserById(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException))]
        public void GetUserById_Should_ThrowEntityNotFoundException_When_UserDoesNotExist()
        {
            // Arrange
            var userId = 99;

            // Act
            _userRepository.GetUserById(userId);

            // Assert - [ExpectedException] handles this
        }

        [TestMethod]
        public void GetUserByUsername_Should_ReturnUser_When_UserExists()
        {
            // Arrange
            var username = "testuser1";

            // Act
            var result = _userRepository.GetUserByUsername(username);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(username, result.Username);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException))]
        public void GetUserByUsername_Should_ThrowEntityNotFoundException_When_UserDoesNotExist()
        {
            // Arrange
            var username = "nonexistentuser";

            // Act
            _userRepository.GetUserByUsername(username);

            // Assert - [ExpectedException] handles this
        }

        [TestMethod]
        public void UserExists_Should_ReturnTrue_When_UserExists()
        {
            // Arrange
            var username = "testuser1";

            // Act
            var result = _userRepository.UserExists(username);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UserExists_Should_ReturnFalse_When_UserDoesNotExist()
        {
            // Arrange
            var username = "nonexistentuser";

            // Act
            var result = _userRepository.UserExists(username);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void EmailExists_Should_ReturnTrue_When_EmailIsRegistered()
        {
            // Arrange
            var email = "test1@example.com";

            // Act
            var result = _userRepository.EmailExists(email);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EmailExists_Should_ReturnFalse_When_EmailIsNotRegistered()
        {
            // Arrange
            var email = "nonexistent@example.com";

            // Act
            var result = _userRepository.EmailExists(email);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RegisterUser_Should_AddUser_When_UserIsValid()
        {
            // Arrange
            var newUser = new User
            {
                Id = 3,
                Username = "newuser",
                Password = "NewPassword!",
                Email = "newuser@example.com",
                FirstName = "New",
                LastName = "User"
            };

            // Act
            _userRepository.RegisterUser(newUser);

            // Assert
            var user = _userRepository.GetUserById(3);
            Assert.IsNotNull(user);
            Assert.AreEqual("newuser", user.Username);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateEntityException))]
        public void RegisterUser_Should_ThrowDuplicateEntityException_When_EmailAlreadyExists()
        {
            // Arrange
            var newUser = new User
            {
                Id = 4,
                Username = "anotheruser",
                Password = "AnotherPassword!",
                Email = "test1@example.com", // This email already exists
                FirstName = "Another",
                LastName = "User"
            };

            // Act
            _userRepository.RegisterUser(newUser);

            // Assert - [ExpectedException] handles this
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateEntityException))]
        public void RegisterUser_Should_ThrowDuplicateEntityException_When_UsernameAlreadyExists()
        {
            // Arrange
            var newUser = new User
            {
                Id = 4,
                Username = "testuser1", // This username already exists
                Password = "AnotherPassword!",
                Email = "anotheruser@example.com",
                FirstName = "Another",
                LastName = "User"
            };

            // Act
            _userRepository.RegisterUser(newUser);

            // Assert - [ExpectedException] handles this
        }

        [TestMethod]
        public void UpdateUser_Should_UpdateUser_When_UserExists()
        {
            // Arrange
            var userToUpdate = new User
            {
                Id = 1,
                Username = "testuser1",
                Password = "UpdatedPassword!",
                Email = "updated@example.com",
                FirstName = "UpdatedFirst",
                LastName = "UpdatedLast"
            };

            // Act
            _userRepository.UpdateUser(userToUpdate);

            // Assert
            var user = _userRepository.GetUserById(1);
            Assert.AreEqual("UpdatedPassword!", user.Password);
            Assert.AreEqual("updated@example.com", user.Email);
            Assert.AreEqual("UpdatedFirst", user.FirstName);
            Assert.AreEqual("UpdatedLast", user.LastName);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException))]
        public void UpdateUser_Should_ThrowArgumentNullException_When_UserDoesNotExist()
        {
            // Arrange
            var userToUpdate = new User
            {
                Id = 99, // Non-existing user ID
                Username = "nonexistentuser",
                Password = "SomePassword!",
                Email = "nonexistent@example.com"
            };

            // Act
            _userRepository.UpdateUser(userToUpdate);

            // Assert - [ExpectedException] handles this
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException))]
        public void DeleteUser_Should_SetDeletedAt_When_UserExists()
        {
            // Arrange
            var newUser = new User
            {
                Username = "anotheruser1123456",
                Password = "AnotherPassword!",
                Email = "test1123456@example.com",
                FirstName = "Another",
                LastName = "User"
            };

            _userRepository.RegisterUser(newUser);
            var addedUser = _userRepository.GetUserByUsername(newUser.Username);

            // Act
            _userRepository.DeleteUser(addedUser.Id);

            // Assert
            var user = _userRepository.GetUserById(addedUser.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException))]
        public void DeleteUser_Should_ThrowEntityNotFoundException_When_UserDoesNotExist()
        {
            // Arrange
            var userId = 99;

            // Act
            _userRepository.DeleteUser(userId);

            // Assert - [ExpectedException] handles this
        }

        [TestMethod]
        public void BlockUser_Should_SetIsBlockedTrue_When_UserExists()
        {
            // Arrange
            var userId = 1;

            // Act
            _userRepository.BlockUser(userId);

            // Assert
            var user = _userRepository.GetUserById(userId);
            Assert.IsTrue(user.IsBlocked);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException))]
        public void BlockUser_Should_ThrowEntityNotFoundException_When_UserDoesNotExist()
        {
            // Arrange
            var userId = 99;

            // Act
            _userRepository.BlockUser(userId);

            // Assert - [ExpectedException] handles this
        }

        [TestMethod]
        public void UnblockUser_Should_SetIsBlockedFalse_When_UserExists()
        {
            // Arrange
            var userId = 1;
            _userRepository.BlockUser(userId); // Ensure user is blocked first

            // Act
            _userRepository.UnblockUser(userId);

            // Assert
            var user = _userRepository.GetUserById(userId);
            Assert.IsFalse(user.IsBlocked);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException))]
        public void UnblockUser_Should_ThrowEntityNotFoundException_When_UserDoesNotExist()
        {
            // Arrange
            var userId = 99;

            // Act
            _userRepository.UnblockUser(userId);

            // Assert - [ExpectedException] handles this
        }

        [TestMethod]
        public void PromoteUserToAdmin_Should_SetIsAdminTrue_When_UserExists()
        {
            // Arrange
            var userId = 1;

            // Act
            _userRepository.PromoteUserToAdmin(userId);

            // Assert
            var user = _userRepository.GetUserById(userId);
            Assert.IsTrue(user.IsAdmin);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PromoteUserToAdmin_Should_ThrowArgumentNullException_When_UserIsAlreadyAdmin()
        {
            // Arrange
            var userId = 1;
            _userRepository.PromoteUserToAdmin(userId); // Promote to admin first

            // Act
            _userRepository.PromoteUserToAdmin(userId);

            // Assert - [ExpectedException] handles this
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException))]
        public void PromoteUserToAdmin_Should_ThrowEntityNotFoundException_When_UserDoesNotExist()
        {
            // Arrange
            var userId = 99;

            // Act
            _userRepository.PromoteUserToAdmin(userId);

            // Assert - [ExpectedException] handles this
        }

        [TestMethod]
        public void DemoteUserFromAdmin_Should_SetIsAdminFalse_When_UserIsAdmin()
        {
            // Arrange
            var userId = 1;
            _userRepository.PromoteUserToAdmin(userId); // Ensure user is an admin

            // Act
            _userRepository.DemoteUserFromAdmin(userId);

            // Assert
            var user = _userRepository.GetUserById(userId);
            Assert.IsFalse(user.IsAdmin);
        }

        [TestMethod]
        [ExpectedException(typeof(BadRequestException))]
        public void DemoteUserFromAdmin_Should_ThrowBadRequestException_When_UserIsNotAdmin()
        {
            // Arrange
            var userId = 2; // Not an admin

            // Act
            _userRepository.DemoteUserFromAdmin(userId);

            // Assert - [ExpectedException] handles this
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException))]
        public void DemoteUserFromAdmin_Should_ThrowEntityNotFoundException_When_UserDoesNotExist()
        {
            // Arrange
            var userId = 99;

            // Act
            _userRepository.DemoteUserFromAdmin(userId);

            // Assert - [ExpectedException] handles this
        }

        [TestMethod]
        public void UploadProfilePicture_Should_UpdateProfilePictureUrl_When_UserExists()
        {
            // Arrange
            var username = "testuser1";
            var newProfilePictureUrl = "/images/newprofile.jpg";

            // Act
            _userRepository.UploadProfilePicture(username, newProfilePictureUrl);

            // Assert
            var user = _userRepository.GetUserByUsername(username);
            Assert.AreEqual(newProfilePictureUrl, user.ProfilePictureUrl);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException))]
        public void UploadProfilePicture_Should_ThrowEntityNotFoundException_When_UserDoesNotExist()
        {
            // Arrange
            var username = "nonexistentuser";
            var newProfilePictureUrl = "/images/newprofile.jpg";

            // Act
            _userRepository.UploadProfilePicture(username, newProfilePictureUrl);

            // Assert - [ExpectedException] handles this
        }
    }

    public static class TestHelper
    {
        public static List<User> GetTestUsers()
        {
            return new List<User>
                {
                    new User { Id = 1, Username = "testuser1", Email = "test1@example.com", FirstName = "First1", LastName = "Last1" },
                    new User { Id = 2, Username = "testuser2", Email = "test2@example.com", FirstName = "First2", LastName = "Last2" }
                };
        }

        public static Mock<DbSet<T>> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();
            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>(sourceList.Add);
            return dbSet;
        }
    }
}
