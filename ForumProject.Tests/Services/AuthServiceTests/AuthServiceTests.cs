using ForumProject.Models;
using ForumProject.Models.DTOs;
using ForumProject.Repositories.Contracts;
using ForumProject.Services;
using ForumProject.Services.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Security.Authentication;
using ForumProject.Exceptions;
using ForumProject.Helpers;

namespace ForumProject.Tests.Services.AuthServiceTests

{
    [TestClass]
    public class AuthServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IAuthManager> _authManagerMock;
        private Mock<IModelMapper> _modelMapperMock;
        private AuthService _authService;

        [TestInitialize]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _authManagerMock = new Mock<IAuthManager>();
            _modelMapperMock = new Mock<IModelMapper>();
            _authService = new AuthService(_userRepositoryMock.Object, _authManagerMock.Object, _modelMapperMock.Object);
        }

        [TestMethod]
        public void Authenticate_ShouldReturnUserResponseDto_WhenCredentialsAreValid()
        {
            // Arrange
            var credentials = "testuser:password";
            var user = TestHelper.GetTestUser();
            var userResponseDto = TestHelper.GetUserResponseDto();

            _userRepositoryMock.Setup(x => x.GetUserByUsername("testuser")).Returns(user);
            _authManagerMock.Setup(x => x.VerifyPassword("testuser", "password")).Returns(true);
            _modelMapperMock.Setup(x => x.ToUserResponseDto(user)).Returns(userResponseDto);

            // Act
            var result = _authService.Authenticate(credentials);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userResponseDto.Username, result.Username);
        }

        [TestMethod]
        public void Authenticate_ShouldThrowInvalidCredentialException_WhenCredentialsAreInvalid()
        {
            // Arrange
            var credentials = "testuser:wrongpassword";
            var user = TestHelper.GetTestUser();

            _userRepositoryMock.Setup(x => x.GetUserByUsername("testuser")).Returns(user);
            _authManagerMock.Setup(x => x.VerifyPassword("testuser", "wrongpassword")).Returns(false);

            // Act & Assert
            Assert.ThrowsException<InvalidCredentialException>(() => _authService.Authenticate(credentials));
        }

        [TestMethod]
        public void Register_ShouldReturnUserResponseDto_WhenUserIsRegistered()
        {
            // Arrange
            var userRequestDto = TestHelper.GetUserRequestDto();
            var user = TestHelper.GetTestUser();
            var userResponseDto = TestHelper.GetUserResponseDto();

            _userRepositoryMock.Setup(x => x.UserExists(userRequestDto.Username)).Returns(false);
            _userRepositoryMock.Setup(x => x.EmailExists(userRequestDto.Email)).Returns(false);
            _modelMapperMock.Setup(x => x.ToUser(userRequestDto)).Returns(user);
            _userRepositoryMock.Setup(x => x.RegisterUser(user));
            _userRepositoryMock.Setup(x => x.GetUserByUsername(userRequestDto.Username)).Returns(user);
            _modelMapperMock.Setup(x => x.ToUserResponseDto(user)).Returns(userResponseDto);

            // Act
            var result = _authService.Register(userRequestDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userResponseDto.Username, result.Username);
        }

        [TestMethod]
        public void Register_ShouldThrowDuplicateEntityException_WhenUsernameExists()
        {
            // Arrange
            var userRequestDto = TestHelper.GetUserRequestDto();

            _userRepositoryMock.Setup(x => x.UserExists(userRequestDto.Username)).Returns(true);

            // Act & Assert
            Assert.ThrowsException<DuplicateEntityException>(() => _authService.Register(userRequestDto));
        }

        [TestMethod]
        public void Register_ShouldThrowDuplicateEntityException_WhenEmailExists()
        {
            // Arrange
            var userRequestDto = TestHelper.GetUserRequestDto();

            _userRepositoryMock.Setup(x => x.EmailExists(userRequestDto.Email)).Returns(true);

            // Act & Assert
            Assert.ThrowsException<DuplicateEntityException>(() => _authService.Register(userRequestDto));
        }
    }
}
