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
    public class Edit_Should
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
        public void ReturnViewWithModel_When_ValidData()
        {
            // Arrange
            var postViewModel = TestHelper.GetPostViewModel();
            var postEntity = TestHelper.GetTestPost();

            _postServiceMock.Setup(x => x.GetPostById(postEntity.Id)).Returns(postEntity);
            _modelMapperMock.Setup(x => x.ToPostViewModel(postEntity)).Returns(postViewModel);

            // Act
            var result = _controller.Edit(postEntity.Id) as ViewResult;

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
                var result = _controller.Edit(postId) as NotFoundResult;

                // Assert
                Assert.Fail("Exception was expected but not thrown.");
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsTrue(ex is EntityNotFoundException);
                Assert.AreEqual("Post not found", ex.Message);
            }
        }

        [TestMethod]
        public void ReturnViewWithModel_When_ModelStateIsInvalid()
        {
            // Arrange
            var postViewModel = TestHelper.GetPostViewModel();
            _controller.ModelState.AddModelError("Title", "Title is required");

            // Act
            var result = _controller.Edit(postViewModel.Id, postViewModel) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(postViewModel, result.Model);
        }

        [TestMethod]
        public void ReturnNotFound_When_PostToUpdateDoesNotExist()
        {
            // Arrange
            var postViewModel = TestHelper.GetPostViewModel();
            _postServiceMock.Setup(x => x.GetPostById(postViewModel.Id)).Returns((Post)null);

            // Act
            try
            {
                var result = _controller.Edit(postViewModel.Id, postViewModel) as NotFoundResult;

                // Assert
                Assert.Fail("Exception was expected but not thrown.");
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsTrue(ex is Exception);
                Assert.AreEqual("Post not found", ex.Message);
            }
        }

        [TestMethod]
        public void UpdatePostAndRedirectToDetails_When_ValidData()
        {
            // Arrange
            var postViewModel = TestHelper.GetPostViewModel();
            var postEntity = TestHelper.GetTestPost();

            _postServiceMock.Setup(x => x.GetPostById(postEntity.Id)).Returns(postEntity);

            // Act
            var result = _controller.Edit(postEntity.Id, postViewModel) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Details", result.ActionName);
            Assert.AreEqual(postEntity.Id, result.RouteValues["id"]);
            _postServiceMock.Verify(x => x.UpdatePost(It.IsAny<Post>()), Times.Once);
        }

        [TestMethod]
        public void ReturnErrorView_When_GeneralExceptionIsThrown()
        {
            // Arrange
            var postViewModel = TestHelper.GetPostViewModel();
            var postEntity = TestHelper.GetTestPost();

            _postServiceMock.Setup(x => x.GetPostById(postEntity.Id)).Returns(postEntity);
            _postServiceMock.Setup(x => x.UpdatePost(It.IsAny<Post>())).Throws(new Exception("General error"));

            // Act
            try
            {
                var result = _controller.Edit(postEntity.Id, postViewModel) as ViewResult;

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
