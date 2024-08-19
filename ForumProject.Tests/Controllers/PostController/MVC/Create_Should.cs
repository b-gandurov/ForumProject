using ForumProject.Controllers;
using ForumProject.Controllers.API;
using ForumProject.Helpers;
using ForumProject.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ForumProject.Controllers.MVC;
using PostsController = ForumProject.Controllers.MVC.PostsController;
using ForumProject.Services.Contracts;
using ForumProject.Models;
using ForumProject.Exceptions;
namespace ForumProject.Tests.Controllers.PostController.MVC
{
    [TestClass]
    public class Create_Should
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
        public void ReturnCreatedPost_When_ValidData()
        {
            // Arrange
            var postViewModel = TestHelper.GetPostViewModel();
            var postEntity = TestHelper.GetTestPost();
            var user = TestHelper.GetTestUser();

            _userServiceMock.Setup(x => x.GetUserByUsername(postViewModel.Username)).Returns(user);
            _modelMapperMock.Setup(x => x.ToPost(postViewModel)).Returns(postEntity);
            _postServiceMock.Setup(x => x.AddPost(It.IsAny<Post>())).Callback<Post>(p => p.Id = postEntity.Id);
            _modelMapperMock.Setup(x => x.ToPostViewModel(postEntity)).Returns(postViewModel);

            // Act
            var result = _controller.Create(postViewModel) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Details", result.ActionName);
            Assert.AreEqual(postEntity.Id, result.RouteValues["id"]);
        }

        [TestMethod]
        public void ReturnViewWithModel_When_ModelStateIsInvalid()
        {
            // Arrange
            var postViewModel = TestHelper.GetPostViewModel();
            _controller.ModelState.AddModelError("Title", "Title is required");

            // Act
            var result = _controller.Create(postViewModel) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(postViewModel, result.Model);
        }

        [TestMethod]
        public void AddModelErrorAndReturnView_When_DuplicateEntityExceptionIsThrown()
        {
            // Arrange
            var postViewModel = TestHelper.GetPostViewModel();
            var postEntity = TestHelper.GetTestPost();
            var user = TestHelper.GetTestUser();

            _userServiceMock.Setup(x => x.GetUserByUsername(postViewModel.Username)).Returns(user);
            _modelMapperMock.Setup(x => x.ToPost(postViewModel)).Returns(postEntity);
            _postServiceMock.Setup(x => x.AddPost(It.IsAny<Post>())).Throws(new DuplicateEntityException("Duplicate entity"));

            // Act
            var result = _controller.Create(postViewModel) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(postViewModel, result.Model);
            Assert.IsTrue(_controller.ModelState.ContainsKey("Title"));
            Assert.AreEqual("Duplicate entity", _controller.ModelState["Title"].Errors[0].ErrorMessage);
        }

        //[TestMethod]
        //public void ReturnErrorView_When_GeneralExceptionIsThrown()
        //{
        //    // Arrange
        //    var postViewModel = TestHelper.GetPostViewModel();
        //    var postEntity = TestHelper.GetTestPost();
        //    var user = TestHelper.GetTestUser();

        //    _userServiceMock.Setup(x => x.GetUserByUsername(postViewModel.Username)).Returns(user);
        //    _modelMapperMock.Setup(x => x.ToPost(postViewModel)).Returns(postEntity);
        //    _postServiceMock.Setup(x => x.AddPost(It.IsAny<Post>())).Throws(new Exception("General error"));

        //    // Act
        //    try
        //    {
        //        var result = _controller.Create(postViewModel) as ViewResult;

        //        // Assert
        //        Assert.Fail("Exception was expected but not thrown.");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Assert
        //        Assert.IsTrue(ex is Exception);
        //        Assert.AreEqual("General error", ex.Message);
        //    }
        //}
    }
}
