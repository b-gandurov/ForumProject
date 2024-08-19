using ForumProject.Controllers.MVC;
using ForumProject.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumProject.Tests.Controllers.AuthenticationControllerTests
{
    [TestClass]
    public class Logout_Should
    {
        private Mock<IAuthService> _authServiceMock;
        private Mock<IUserContextService> _userContextServiceMock;
        private AuthenticationController _controller;
        private Mock<HttpContext> _httpContextMock;
        private Mock<HttpResponse> _httpResponseMock;
        private Mock<IResponseCookies> _responseCookiesMock;

        [TestInitialize]
        public void SetUp()
        {
            _authServiceMock = new Mock<IAuthService>();
            _userContextServiceMock = new Mock<IUserContextService>();
            _controller = new AuthenticationController(_authServiceMock.Object, _userContextServiceMock.Object);

            _httpContextMock = new Mock<HttpContext>();
            _httpResponseMock = new Mock<HttpResponse>();
            _responseCookiesMock = new Mock<IResponseCookies>();

            _httpContextMock.Setup(ctx => ctx.Response).Returns(_httpResponseMock.Object);
            _httpResponseMock.Setup(res => res.Cookies).Returns(_responseCookiesMock.Object);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = _httpContextMock.Object
            };
        }

        [TestMethod]
        public void DeleteJwtCookie_And_RedirectToLogin()
        {
            // Act
            var result = _controller.Logout() as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Login", result.ActionName);
            _responseCookiesMock.Verify(cookies => cookies.Delete("jwt"), Times.Once);
        }
    }

}
