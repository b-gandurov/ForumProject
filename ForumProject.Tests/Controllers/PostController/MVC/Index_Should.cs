using ForumProject.Controllers.MVC;
using ForumProject.Helpers;
using ForumProject.Models;
using ForumProject.Models.Enums;
using ForumProject.Models.QueryParameters;
using ForumProject.Models.ViewModels;
using ForumProject.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ForumProject.Tests.Controllers.PostController.MVC
{
    [TestClass]
    public class Index_Should
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
        public void ReturnViewWithFilteredPosts_When_FilterParametersAreProvided()
        {
            // Arrange
            var filterParameters = new PostQueryParameters
            {
                Title = "Test",
                Category = PostCategory.Story,
                PageNumber = 1,
                PageSize = 10
            };

            var posts = new List<Post> { TestHelper.GetTestPost() };
            var postViewModels = posts.Select(p => TestHelper.GetPostViewModel()).ToList();

            _postServiceMock.Setup(x => x.FilterBy(filterParameters)).Returns(posts);
            _postServiceMock.Setup(x => x.GetTotalCount(filterParameters)).Returns(posts.Count);
            _modelMapperMock.Setup(x => x.ToPostViewModel(It.IsAny<Post>())).Returns(postViewModels.First());

            // Act
            var result = _controller.Index(filterParameters) as ViewResult;
            var resultModel = result.Model as List<PostViewModel>;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(resultModel);
            CollectionAssert.AreEqual(postViewModels, resultModel);
            Assert.AreEqual(posts.Count, _controller.ViewBag.TotalCount);
            Assert.AreEqual(filterParameters.PageSize, _controller.ViewBag.PageSize);
            Assert.AreEqual(filterParameters.PageNumber, _controller.ViewBag.PageNumber);
            Assert.IsNotNull(_controller.ViewBag.Categories);
        }


        [TestMethod]
        public void ReturnViewWithAllPosts_When_NoFilterParametersAreProvided()
        {
            // Arrange
            var filterParameters = new PostQueryParameters
            {
                PageNumber = 1,
                PageSize = 10
            };

            var posts = new List<Post> { TestHelper.GetTestPost() };
            var postViewModels = posts.Select(p => TestHelper.GetPostViewModel()).ToList();

            _postServiceMock.Setup(x => x.FilterBy(filterParameters)).Returns(posts);
            _postServiceMock.Setup(x => x.GetTotalCount(filterParameters)).Returns(posts.Count);
            _modelMapperMock.Setup(x => x.ToPostViewModel(It.IsAny<Post>())).Returns(postViewModels.First());

            // Act
            var result = _controller.Index(filterParameters) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            //Assert.AreEqual(postViewModels, result.Model);
            Assert.AreEqual(posts.Count, _controller.ViewBag.TotalCount);
            Assert.AreEqual(filterParameters.PageSize, _controller.ViewBag.PageSize);
            Assert.AreEqual(filterParameters.PageNumber, _controller.ViewBag.PageNumber);
            Assert.IsNotNull(_controller.ViewBag.Categories);
        }

        [TestMethod]
        public void ReturnViewWithEmptyList_When_NoPostsMatchFilterParameters()
        {
            // Arrange
            var filterParameters = new PostQueryParameters
            {
                Title = "NonExistingTitle",
                PageNumber = 1,
                PageSize = 10
            };

            var posts = new List<Post>();
            var postViewModels = new List<PostViewModel>();

            _postServiceMock.Setup(x => x.FilterBy(filterParameters)).Returns(posts);
            _postServiceMock.Setup(x => x.GetTotalCount(filterParameters)).Returns(posts.Count);
            _modelMapperMock.Setup(x => x.ToPostViewModel(It.IsAny<Post>())).Returns((PostViewModel)null);

            // Act
            var result = _controller.Index(filterParameters) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsInstanceOfType(result.Model, typeof(List<PostViewModel>));
            Assert.AreEqual(postViewModels.Count, ((List<PostViewModel>)result.Model).Count);
        }


        [TestMethod]
        public void ReturnErrorView_When_ExceptionIsThrown()
        {
            // Arrange
            var filterParameters = new PostQueryParameters
            {
                PageNumber = 1,
                PageSize = 10
            };

            _postServiceMock.Setup(x => x.FilterBy(filterParameters)).Throws(new Exception("General error"));

            // Act
            try
            {
                var result = _controller.Index(filterParameters) as ViewResult;

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
