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

namespace ForumProject.Tests.Controllers.PostController.MVC
{
    [TestClass]
    public class Delete_Should
    {
        private Mock<IPostService> _postServiceMock;
        private Mock<IModelMapper> _modelMapperMock;
        private Mock<IUserService> _userServiceMock;
        private Mock<ICloudinaryService> _cloudinaryServiceMock;
        private Mock<IUserContextService> _userContextServiceMock;
        private PostsController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _postServiceMock = new Mock<IPostService>();
            _modelMapperMock = new Mock<IModelMapper>();
            _userServiceMock = new Mock<IUserService>();
            _cloudinaryServiceMock = new Mock<ICloudinaryService>();
            _userContextServiceMock = new Mock<IUserContextService>();

            _controller = new PostsController(
                _postServiceMock.Object,
                _modelMapperMock.Object,
                _userServiceMock.Object,
                _userContextServiceMock.Object,
                _cloudinaryServiceMock.Object);
        }

        [TestMethod]
        public void ReturnViewWithPostViewModel_When_PostExists()
        {
            // Arrange
            var postId = 1;
            var postEntity = TestHelper.GetTestPost();
            var postViewModel = TestHelper.GetPostViewModel();

            _postServiceMock.Setup(x => x.GetPostById(postId)).Returns(postEntity);
            _modelMapperMock.Setup(x => x.ToPostViewModel(postEntity)).Returns(postViewModel);

            // Act
            var result = _controller.Delete(postId) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(postViewModel, result.Model);
        }

        [TestMethod]
        public void ReturnNotFound_When_PostDoesNotExist()
        {
            // Arrange
            var postId = 1;
            _postServiceMock.Setup(x => x.GetPostById(postId)).Throws(new EntityNotFoundException("Post not found"));

            // Act
            try
            {
                var result = _controller.Delete(postId) as NotFoundResult;

                // Assert
                Assert.Fail("Exception was expected but not thrown.");
            }
            catch (EntityNotFoundException ex)
            {
                // Assert
                Assert.IsTrue(ex is EntityNotFoundException);
                Assert.AreEqual("Post not found", ex.Message);
            }
        }

        [TestMethod]
        public void DeletePostAndRedirectToIndex_When_PostExists()
        {
            // Arrange
            var postId = 1;
            var postEntity = TestHelper.GetTestPost();

            _postServiceMock.Setup(x => x.GetPostById(postId)).Returns(postEntity);

            // Act
            var result = _controller.DeleteConfirmed(postId) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            _postServiceMock.Verify(x => x.DeletePost(postId), Times.Once);
        }

        [TestMethod]
        public void ReturnNotFound_When_PostToDeleteDoesNotExist()
        {
            // Arrange
            var postId = 1;

            _postServiceMock.Setup(x => x.GetPostById(postId)).Returns((Post)null);

            // Act
            var result = _controller.DeleteConfirmed(postId) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ReturnErrorView_When_GeneralExceptionIsThrown()
        {
            // Arrange
            var postId = 1;
            var errorMessage = "General error";
            _postServiceMock.Setup(x => x.GetPostById(postId)).Throws(new Exception(errorMessage));

            // Act
            try
            {
                var result = _controller.Delete(postId) as ViewResult;

                // Assert
                Assert.Fail("Exception was expected but not thrown.");
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsTrue(ex is Exception);
                Assert.AreEqual(errorMessage, ex.Message);
            }
        }
    }
}
