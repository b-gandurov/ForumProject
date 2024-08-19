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
    public class Details_Should
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
            var result = _controller.Details(postId) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(postViewModel, result.Model);
            Assert.AreEqual(postEntity.Id, result.ViewData["id"]);
        }
    }
}
