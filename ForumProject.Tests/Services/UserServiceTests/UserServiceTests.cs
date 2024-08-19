using ForumProject.Exceptions;
using ForumProject.Models;
using ForumProject.Models.QueryParameters;
using ForumProject.Repositories.Contracts;
using ForumProject.Services;
using Moq;

namespace ForumProject.Tests.Services.UserServiceTests
{
    [TestClass]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private UserService _userService;

        [TestInitialize]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
        }

        [TestMethod]
        public void RegisterUser_Should_CallRepositoryMethod()
        {
            // Arrange
            var user = TestHelper.GetTestUser();

            // Act
            _userService.RegisterUser(user);

            // Assert
            _userRepositoryMock.Verify(x => x.RegisterUser(user), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BlockUser_Should_ThrowArgumentNullException_When_UserDoesNotExist()
        {
            // Arrange
            var userId = 1;
            _userRepositoryMock.Setup(x => x.BlockUser(userId)).Throws(new ArgumentNullException());

            // Act
            _userService.BlockUser(userId);

            // Assert - [ExpectedException] handles this
        }

        [TestMethod]
        public void DeleteUser_Should_CallRepositoryMethod()
        {
            // Arrange
            var userId = 1;

            // Act
            _userService.DeleteUser(userId);

            // Assert
            _userRepositoryMock.Verify(x => x.DeleteUser(userId), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException))]
        public void GetUserById_Should_ThrowEntityNotFoundException_When_UserDoesNotExist()
        {
            // Arrange
            var userId = 1;
            _userRepositoryMock.Setup(x => x.GetUserById(userId)).Throws(new EntityNotFoundException(""));

            // Act
            _userService.GetUserById(userId);

            // Assert - [ExpectedException] handles this
        }

        [TestMethod]
        public void GetUserByUsername_Should_ReturnUser_When_UserExists()
        {
            // Arrange
            var username = "testUser";
            var expectedUser = TestHelper.GetTestUser();
            _userRepositoryMock.Setup(x => x.GetUserByUsername(username)).Returns(expectedUser);

            // Act
            var result = _userService.GetUserByUsername(username);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedUser, result);
        }

        [TestMethod]
        public void SearchUsersBy_Should_ReturnUsers_When_UsersExist()
        {
            // Arrange
            var parameters = new UserQueryParameters();
            var expectedUsers = new List<User> { TestHelper.GetTestUser() };
            _userRepositoryMock.Setup(x => x.SearchUsersBy(parameters)).Returns(expectedUsers);

            // Act
            var result = _userService.SearchUsersBy(parameters);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedUsers.Count(), result.Count());
            Assert.AreEqual(expectedUsers, result);
        }

        [TestMethod]
        public void GetAllUsers_Should_ReturnAllUsers()
        {
            // Arrange
            var expectedUsers = new List<User> { TestHelper.GetTestUser() };
            _userRepositoryMock.Setup(x => x.GetAllUsers()).Returns(expectedUsers);

            // Act
            var result = _userService.GetAllUsers();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedUsers.Count(), result.Count());
            Assert.AreEqual(expectedUsers, result);
        }

        [TestMethod]
        public void UnblockUser_Should_CallRepositoryMethod()
        {
            // Arrange
            var userId = 1;

            // Act
            _userService.UnblockUser(userId);

            // Assert
            _userRepositoryMock.Verify(x => x.UnblockUser(userId), Times.Once);
        }

        [TestMethod]
        public void UpdateUser_Should_CallRepositoryMethod()
        {
            // Arrange
            var user = TestHelper.GetTestUser();

            // Act
            _userService.UpdateUser(user);

            // Assert
            _userRepositoryMock.Verify(x => x.UpdateUser(user), Times.Once);
        }

        [TestMethod]
        public void PromoteUserToAdmin_Should_CallRepositoryMethod()
        {
            // Arrange
            var userId = 1;

            // Act
            _userService.PromoteUserToAdmin(userId);

            // Assert
            _userRepositoryMock.Verify(x => x.PromoteUserToAdmin(userId), Times.Once);
        }

        [TestMethod]
        public void DemoteUserFromAdmin_Should_CallRepositoryMethod()
        {
            // Arrange
            var userId = 1;

            // Act
            _userService.DemoteUserFromAdmin(userId);

            // Assert
            _userRepositoryMock.Verify(x => x.DemoteUserFromAdmin(userId), Times.Once);
        }
    }
}
