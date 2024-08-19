using ForumProject.Exceptions;
using ForumProject.Helpers;
using ForumProject.Models;
using ForumProject.Models.DTOs;
using ForumProject.Repositories.Contracts;
using ForumProject.Services;
using ForumProject.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;

namespace ForumProject.Tests.Services.JWTAuthServiceTests

{
    [TestClass]
    public class JWTAuthServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IAuthManager> _authManagerMock;
        private Mock<IModelMapper> _modelMapperMock;
        private IConfiguration _configuration;
        private JWTAuthService _authService;

        [TestInitialize]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _authManagerMock = new Mock<IAuthManager>();
            _modelMapperMock = new Mock<IModelMapper>();
            _configuration = TestHelper.GetJwtConfiguration();
            _authService = new JWTAuthService(_configuration, _userRepositoryMock.Object, _modelMapperMock.Object, _authManagerMock.Object);
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
        public void GenerateToken_ShouldReturnValidToken_WhenUserIsValid()
        {
            // Arrange
            var userResponseDto = TestHelper.GetUserResponseDto();

            // Act
            var token = _authService.GenerateToken(userResponseDto);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            // Assert
            tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
            Assert.IsNotNull(validatedToken);
        }

        [TestMethod]
        public void Register_ShouldReturnUserResponseDto_WhenUserIsRegistered()
        {
            // Arrange
            var userRequestDto = TestHelper.GetUserRequestDto();
            var user = TestHelper.GetTestUser();
            var userResponseDto = TestHelper.GetUserResponseDto();

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
        public void ValidateToken_ShouldReturnTrue_WhenTokenIsValid()
        {
            // Arrange
            var userResponseDto = TestHelper.GetUserResponseDto();
            var token = _authService.GenerateToken(userResponseDto);

            // Act
            var result = _authService.ValidateToken(token);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateToken_ShouldReturnFalse_WhenTokenIsInvalid()
        {
            // Arrange
            var invalidToken = "invalid-token";

            // Act
            var result = _authService.ValidateToken(invalidToken);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
