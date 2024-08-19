using ForumProject.Controllers;
using ForumProject.Controllers.MVC;
using ForumProject.Exceptions;
using ForumProject.Models;
using ForumProject.Models.DTOs;
using ForumProject.Models.ViewModels;
using ForumProject.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ForumProject.Tests.Controllers.AuthenticationControllerTests
{
    [TestClass]
    public class Register_Should
    {
        private Mock<IAuthService> _authServiceMock;
        private Mock<IUserContextService> _userContextServiceMock;
        private AuthenticationController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _authServiceMock = new Mock<IAuthService>();
            _userContextServiceMock = new Mock<IUserContextService>();
            _controller = new AuthenticationController(_authServiceMock.Object, _userContextServiceMock.Object);

            // Necessary to mock HttpContext
            var httpContextMock = new Mock<HttpContext>();
            var responseMock = new Mock<HttpResponse>();
            var responseCookiesMock = new Mock<IResponseCookies>();

            responseMock.Setup(r => r.Cookies).Returns(responseCookiesMock.Object);
            httpContextMock.Setup(h => h.Response).Returns(responseMock.Object);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContextMock.Object
            };
        }

        [TestMethod]
        public void ReturnRedirectToActionResult_When_ValidRegistration()
        {
            // Arrange
            var model = new RegisterViewModel { Username = "testuser", Password = "password" };
            var user = new User { Username = "testuser" };
            var userResponseDto = new UserResponseDto { Username = "testuser" };
            var token = "valid-token";

            _authServiceMock.Setup(x => x.Authenticate(It.IsAny<string>())).Returns(userResponseDto);
            _authServiceMock.Setup(x => x.GenerateToken(It.IsAny<UserResponseDto>())).Returns(token);

            // Act
            var result = _controller.Register(model) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
        }

        [TestMethod]
        public void ReturnViewWithModelError_When_DuplicateEntityExceptionIsThrown()
        {
            // Arrange
            var model = new RegisterViewModel { Username = "duplicateuser", Password = "password" };
            _authServiceMock.Setup(x => x.Register(It.IsAny<UserRequestDto>())).Throws(new DuplicateEntityException("Username already exists"));

            // Act
            var result = _controller.Register(model) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(model, result.Model);
            Assert.IsTrue(_controller.ModelState.ContainsKey("Username"));
            Assert.AreEqual("Username already exists", _controller.ModelState["Username"].Errors[0].ErrorMessage);
        }

    }
}
