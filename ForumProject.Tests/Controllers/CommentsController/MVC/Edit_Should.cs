using ForumProject.Controllers.MVC;
using ForumProject.Helpers;
using ForumProject.Models;
using ForumProject.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace ForumProject.Tests.Controllers.CommentsController.MVC
{
    [TestClass]
    public class Edit_Post_Should
    {
        private Mock<ICommentService> _commentServiceMock;
        private Mock<IModelMapper> _modelMapperMock;
        private Mock<IUserService> _userServiceMock;
        private Mock<IUserContextService> _userContextServiceMock;
        private CommentController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _commentServiceMock = new Mock<ICommentService>();
            _modelMapperMock = new Mock<IModelMapper>();
            _userServiceMock = new Mock<IUserService>();
            _userContextServiceMock = new Mock<IUserContextService>();

            _controller = new CommentController(
                _commentServiceMock.Object,
                _userContextServiceMock.Object,
                _modelMapperMock.Object,
                _userServiceMock.Object);
        }

        [TestMethod]
        public void ReturnRedirectToActionResult_When_ValidData()
        {
            // Arrange
            var commentViewModel = TestHelper.GetCommentViewModel();
            var commentEntity = TestHelper.GetTestComment();
            var user = TestHelper.GetTestUser();

            _userServiceMock.Setup(x => x.GetUserByUsername(commentViewModel.User.Username)).Returns(user);
            _modelMapperMock.Setup(x => x.ToComment(commentViewModel)).Returns(commentEntity);
            _commentServiceMock.Setup(x => x.UpdateComment(It.IsAny<Comment>()));

            var httpContext = new DefaultHttpContext();
            httpContext.Items["User"] = user;
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            var result = _controller.Edit(commentViewModel) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Details", result.ActionName);
            Assert.AreEqual("Posts", result.ControllerName);
            Assert.AreEqual(commentEntity.PostId, result.RouteValues["id"]);
        }



        [TestMethod]
        public void ReturnNotFound_When_CommentDoesNotExist()
        {
            // Arrange
            var commentViewModel = TestHelper.GetCommentViewModel();
            _commentServiceMock.Setup(x => x.GetCommentById(commentViewModel.Id)).Returns((Comment)null);

            // Act
            var result = _controller.Edit(commentViewModel.Id) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ReturnRedirectToActionResult_When_InvalidModelState()
        {
            // Arrange
            var commentViewModel = TestHelper.GetCommentViewModel();
            _controller.ModelState.AddModelError("Content", "Required");

            // Act
            var result = _controller.Edit(commentViewModel) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Details", result.ActionName);
            Assert.AreEqual("Posts", result.ControllerName);
            Assert.AreEqual(commentViewModel.PostId, result.RouteValues["id"]);
        }
    }
}