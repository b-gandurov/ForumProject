using ForumProject.Controllers.MVC;
using ForumProject.Helpers;
using ForumProject.Models;
using ForumProject.Models.ViewModels;
using ForumProject.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace ForumProject.Tests.Controllers.HomeControllerTests
{
    [TestClass]
    public class HomeControllerTests
    {
        private Mock<IPostService> _postServiceMock;
        private Mock<IUserService> _userServiceMock;
        private Mock<IModelMapper> _modelMapperMock;
        private Mock<IUserContextService> _userContextServiceMock;
        private HomeController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _postServiceMock = new Mock<IPostService>();
            _userServiceMock = new Mock<IUserService>();
            _modelMapperMock = new Mock<IModelMapper>();
            _userContextServiceMock = new Mock<IUserContextService>();

            _controller = new HomeController(
                _userContextServiceMock.Object,
                _postServiceMock.Object,
                _userServiceMock.Object,
                _modelMapperMock.Object);
        }

        [TestMethod]
        public void Index_Should_ReturnViewWithCorrectData()
        {
            // Arrange
            var topReactedPosts = new List<Post> { TestHelper.GetTestPost() };
            var topCommentedPosts = new List<Post> { TestHelper.GetTestPost() };
            var recentPosts = new List<Post> { TestHelper.GetTestPost() };
            var allUsers = new List<User> { TestHelper.GetTestUser() };
            var allPosts = new List<Post> { TestHelper.GetTestPost() };

            _postServiceMock.Setup(x => x.GetTopReactedPosts(10)).Returns(topReactedPosts);
            _postServiceMock.Setup(x => x.GetTopCommentedPosts(10)).Returns(topCommentedPosts);
            _postServiceMock.Setup(x => x.GetRecentPosts(10)).Returns(recentPosts);
            _userServiceMock.Setup(x => x.GetAllUsers()).Returns(allUsers);
            _postServiceMock.Setup(x => x.GetAllPosts()).Returns(allPosts);
            _modelMapperMock.Setup(x => x.ToPostViewModel(It.IsAny<Post>())).Returns(TestHelper.GetPostViewModel());

            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(topReactedPosts.Count, ((IEnumerable<PostViewModel>)_controller.ViewBag.TopReactedPosts).Count());
            Assert.AreEqual(topCommentedPosts.Count, ((IEnumerable<Post>)_controller.ViewBag.TopCommentedPosts).Count());
            Assert.AreEqual(recentPosts.Count, ((IEnumerable<Post>)_controller.ViewBag.RecentPosts).Count());
            Assert.AreEqual(allUsers.Count, ((IEnumerable<User>)_controller.ViewBag.TotalUsers).Count());
            Assert.AreEqual(allPosts.Count, ((IEnumerable<Post>)_controller.ViewBag.TotalPosts).Count());
        }

        [TestMethod]
        public void About_Should_ReturnView()
        {
            // Act
            var result = _controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
