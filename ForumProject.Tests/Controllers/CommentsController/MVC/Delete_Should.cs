using ForumProject.Controllers;
using ForumProject.Controllers.MVC;
using ForumProject.Exceptions;
using ForumProject.Helpers;
using ForumProject.Models;
using ForumProject.Models.ViewModels;
using ForumProject.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace ForumProject.Tests.Controllers.CommentsController.MVC
{
    [TestClass]
    public class Delete_Should
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
        public void ReturnNotFound_When_CommentDoesNotExist()
        {
            // Arrange
            var commentId = 1;
            _commentServiceMock.Setup(x => x.GetCommentById(commentId)).Returns((Comment)null);

            // Act
            var result = _controller.Delete(commentId) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ReturnViewWithComment_When_CommentExists()
        {
            // Arrange
            var commentId = 1;
            var comment = TestHelper.GetTestComment();
            _commentServiceMock.Setup(x => x.GetCommentById(commentId)).Returns(comment);

            // Act
            var result = _controller.Delete(commentId) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(comment, result.Model);
        }

        [TestMethod]
        public void ReturnRedirectToActionResult_When_CommentDeleted()
        {
            // Arrange
            var commentId = 1;
            var comment = TestHelper.GetTestComment();
            _commentServiceMock.Setup(x => x.GetCommentById(commentId)).Returns(comment);

            // Act
            var result = _controller.DeleteConfirmed(commentId) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Details", result.ActionName);
            Assert.AreEqual("Posts", result.ControllerName);
            Assert.AreEqual(comment.PostId, result.RouteValues["id"]);
        }

        [TestMethod]
        public void ReturnErrorView_When_GeneralExceptionIsThrown()
        {
            // Arrange
            var commentId = 1;
            var comment = TestHelper.GetTestComment();
            _commentServiceMock.Setup(x => x.GetCommentById(commentId)).Returns(comment);
            _commentServiceMock.Setup(x => x.DeleteComment(commentId)).Throws(new Exception("General error"));

            // Act
            try
            {
                var result = _controller.DeleteConfirmed(commentId) as ViewResult;

                // Assert
                Assert.Fail("Exception was expected but not thrown.");
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsTrue(ex is Exception);
                Assert.AreEqual("General error", ex.Message);
            }
        }
    }
}
