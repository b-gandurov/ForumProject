using ForumProject.Controllers.MVC;
using ForumProject.Exceptions;
using ForumProject.Helpers;
using ForumProject.Models;
using ForumProject.Models.ViewModels;
using ForumProject.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace ForumProject.Tests.Controllers.CommentsController.MVC
{
    [TestClass]
    public class Create_Should
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

        //[TestMethod]
        //public void ReturnRedirectToActionResult_When_ValidData()
        //{
        //    // Arrange
        //    var commentViewModel = TestHelper.GetCommentViewModel();
        //    var commentEntity = TestHelper.GetTestComment();
        //    var user = TestHelper.GetTestUser();

        //    _userServiceMock.Setup(x => x.GetUserByUsername(commentViewModel.User.Username)).Returns(user);
        //    _modelMapperMock.Setup(x => x.ToComment(commentViewModel)).Returns(commentEntity);
        //    _commentServiceMock.Setup(x => x.AddComment(It.IsAny<Comment>())).Callback<Comment>(c => c.Id = commentEntity.Id);

        //    // Act
        //    var result = _controller.Create(commentViewModel) as RedirectToActionResult;

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual("Details", result.ActionName);
        //    Assert.AreEqual("Posts", result.ControllerName);
        //    Assert.AreEqual(commentEntity.PostId, result.RouteValues["id"]);
        //}


        [TestMethod]
        public void ReturnRedirectToActionResult_When_ModelStateIsInvalid()
        {
            // Arrange
            var commentViewModel = TestHelper.GetCommentViewModel();
            _controller.ModelState.AddModelError("Content", "Required");

            // Act
            var result = _controller.Create(commentViewModel) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Details", result.ActionName);
            Assert.AreEqual("Posts", result.ControllerName);
            Assert.AreEqual(commentViewModel.PostId, result.RouteValues["id"]);
        }

        //[TestMethod]
        //public void ReturnRedirectToActionResult_When_DuplicateEntityExceptionIsThrown()
        //{
        //    // Arrange
        //    var commentViewModel = TestHelper.GetCommentViewModel();
        //    var commentEntity = TestHelper.GetTestComment();
        //    var user = TestHelper.GetTestUser();

        //    _userServiceMock.Setup(x => x.GetUserByUsername(commentViewModel.User.Username)).Returns(user);
        //    _modelMapperMock.Setup(x => x.ToComment(commentViewModel)).Returns(commentEntity);
        //    _commentServiceMock.Setup(x => x.AddComment(It.IsAny<Comment>())).Throws(new DuplicateEntityException("Duplicate comment"));

        //    // Act
        //    var result = _controller.Create(commentViewModel) as RedirectToActionResult;

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual("Details", result.ActionName);
        //    Assert.AreEqual("Posts", result.ControllerName);
        //    Assert.AreEqual(commentViewModel.PostId, result.RouteValues["id"]);
        //}
    }
}
