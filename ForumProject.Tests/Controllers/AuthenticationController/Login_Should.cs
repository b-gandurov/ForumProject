using ForumProject.Controllers.MVC;
using ForumProject.Models;
using ForumProject.Models.DTOs;
using ForumProject.Models.ViewModels;
using ForumProject.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace ForumProject.Tests.Controllers.AuthenticationControllerTests
{
    [TestClass]
    public class Login_Should
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
            var requestMock = new Mock<HttpRequest>();
            var cookiesMock = new Mock<IRequestCookieCollection>();
            var responseCookiesMock = new Mock<IResponseCookies>();

            responseMock.Setup(r => r.Cookies).Returns(responseCookiesMock.Object);
            requestMock.Setup(r => r.Cookies).Returns(cookiesMock.Object);
            httpContextMock.Setup(h => h.Response).Returns(responseMock.Object);
            httpContextMock.Setup(h => h.Request).Returns(requestMock.Object);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContextMock.Object
            };
        }

        [TestMethod]
        public void ReturnRedirectToActionResult_When_ValidLogin()
        {
            // Arrange
            var model = new LoginViewModel { Username = "testuser", Password = "password" };
            var user = new User { Username = "testuser" };
            var userResponseDto = new UserResponseDto { Username = "testuser" };
            var token = "valid-token";


            _authServiceMock.Setup(x => x.Authenticate(It.IsAny<string>())).Returns(userResponseDto);
            _authServiceMock.Setup(x => x.GenerateToken(It.IsAny<UserResponseDto>())).Returns(token);

            var cookiesMock = new Mock<IResponseCookies>();
            cookiesMock.Setup(c => c.Append(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CookieOptions>()));
            cookiesMock.Setup(c => c.Append("ReturnUrl", "test"));
            var requestMock = new Mock<HttpRequest>();
            var requestCookiesMock = new Mock<IRequestCookieCollection>();
            requestCookiesMock.Setup(c => c["ReturnUrl"]).Returns("");
            requestMock.Setup(r => r.Cookies).Returns(requestCookiesMock.Object);
            var responseMock = new Mock<HttpResponse>();
            responseMock.Setup(r => r.Cookies).Returns(cookiesMock.Object);
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(h => h.Response).Returns(responseMock.Object);
            httpContextMock.Setup(h => h.Request).Returns(requestMock.Object);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContextMock.Object
            };

            // Act
            var result = _controller.Login(model) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
        }


        [TestMethod]
        public void ReturnViewResult_When_InvalidModel()
        {
            // Arrange
            var model = new LoginViewModel();
            _controller.ModelState.AddModelError("Username", "Required");

            // Act
            var result = _controller.Login(model) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(model, result.Model);
        }

        [TestMethod]
        public void ReturnViewResult_WithModelError_When_ExceptionThrown()
        {
            // Arrange
            var model = new LoginViewModel { Username = "testuser", Password = "password" };

            _authServiceMock.Setup(x => x.Authenticate(It.IsAny<string>())).Throws(new Exception("Invalid credentials"));

            // Act
            var result = _controller.Login(model) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(model, result.Model);
            Assert.IsTrue(_controller.ModelState.ContainsKey("Error"));
            Assert.AreEqual("Invalid credentials", _controller.ModelState["Error"].Errors[0].ErrorMessage);
        }
    }
}
