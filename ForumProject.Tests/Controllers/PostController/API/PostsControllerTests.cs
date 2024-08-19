using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ForumProject.Controllers.API;
using ForumProject.Helpers;
using ForumProject.Models.DTOs;
using ForumProject.Models.QueryParameters;
using ForumProject.Services.Contracts;
using ForumProject.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace ForumProject.Tests.Controllers.API
{
    [TestClass]
    public class PostsControllerTests
    {
        private Mock<IPostService> _postServiceMock;
        private Mock<IModelMapper> _modelMapperMock;
        private Mock<IAuthService> _authServiceMock;
        private PostsController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _postServiceMock = new Mock<IPostService>();
            _modelMapperMock = new Mock<IModelMapper>();
            _authServiceMock = new Mock<IAuthService>();
            _controller = new PostsController(_postServiceMock.Object, _modelMapperMock.Object, _authServiceMock.Object);
        }

        [TestMethod]
        public void GetAllPosts_ShouldReturnOkResult_WithPostDtos()
        {
            // Arrange
            var posts = new List<Post> { TestHelper.GetTestPost() };
            var postDtos = posts.Select(p => TestHelper.GetPostResponseDto()).ToList();
            _postServiceMock.Setup(s => s.GetAllPosts()).Returns(posts);
            _modelMapperMock.Setup(m => m.ToPostResponseDto(It.IsAny<Post>())).Returns(TestHelper.GetPostResponseDto());

            // Act
            var result = _controller.GetAllPosts() as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(postDtos.Count, ((IEnumerable<PostResponseDto>)result.Value).Count());
        }

        [TestMethod]
        [DataRow(1)]
        public void GetPostById_ShouldReturnOkResult_WithPostDto(int postId)
        {
            // Arrange
            var post = TestHelper.GetTestPost();
            _postServiceMock.Setup(s => s.GetPostById(postId)).Returns(post);
            _modelMapperMock.Setup(m => m.ToPostResponseDto(post)).Returns(TestHelper.GetPostResponseDto());

            // Act
            var result = _controller.GetPostById(postId) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsInstanceOfType(result.Value, typeof(PostResponseDto));
        }

        [TestMethod]
        [DataRow(1)]
        public void GetPostById_ShouldReturnNotFound_WhenPostDoesNotExist(int postId)
        {
            // Arrange
            _postServiceMock.Setup(s => s.GetPostById(It.IsAny<int>())).Returns((Post)null);

            // Act
            var result = _controller.GetPostById(postId) as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        public void AddPost_ShouldReturnCreatedAtActionResult_WithPostDto()
        {
            // Arrange
            var postDto = TestHelper.GetPostRequestDto();
            var post = TestHelper.GetTestPost();
            var user = TestHelper.GetTestUser();
            _modelMapperMock.Setup(m => m.ToPost(postDto)).Returns(post);
            _authServiceMock.Setup(a => a.Authenticate(It.IsAny<string>())).Returns(TestHelper.GetUserResponseDto());
            _postServiceMock.Setup(s => s.AddPost(post));
            _modelMapperMock.Setup(m => m.ToPostResponseDto(post)).Returns(TestHelper.GetPostResponseDto());

            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Items["User"] = user;

            // Act
            var result = _controller.AddPost(postDto) as CreatedAtActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(201, result.StatusCode);
            Assert.IsInstanceOfType(result.Value, typeof(PostResponseDto));
        }

        [TestMethod]
        [DataRow(1)]
        public void UpdatePost_ShouldReturnOkResult_WithUpdatedPostDto(int postId)
        {
            // Arrange
            var postDto = TestHelper.GetPostRequestDto();
            var post = TestHelper.GetTestPost();
            _postServiceMock.Setup(s => s.GetPostById(postId)).Returns(post);
            _modelMapperMock.Setup(m => m.ToPostResponseDto(post)).Returns(TestHelper.GetPostResponseDto());

            // Act
            var result = _controller.UpdatePost(postId, postDto) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsInstanceOfType(result.Value, typeof(PostResponseDto));
        }

        [TestMethod]
        [DataRow(1)]
        public void UpdatePost_ShouldReturnNotFound_WhenPostDoesNotExist(int postId)
        {
            // Arrange
            var postDto = TestHelper.GetPostRequestDto();
            _postServiceMock.Setup(s => s.GetPostById(It.IsAny<int>())).Returns((Post)null);

            // Act
            var result = _controller.UpdatePost(postId, postDto) as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        [DataRow(1)]
        public void DeletePost_ShouldReturnOkResult_WhenPostIsDeleted(int postId)
        {
            // Arrange
            var post = TestHelper.GetTestPost();
            _postServiceMock.Setup(s => s.GetPostById(postId)).Returns(post);

            // Act
            var result = _controller.DeletePost(postId) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        [DataRow(1)]
        public void DeletePost_ShouldReturnNotFound_WhenPostDoesNotExist(int postId)
        {
            // Arrange
            _postServiceMock.Setup(s => s.GetPostById(It.IsAny<int>())).Returns((Post)null);

            // Act
            var result = _controller.DeletePost(postId) as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        public void GetTopPosts_ShouldReturnOkResult_WithPostDtos()
        {
            // Arrange
            var posts = new List<Post> { TestHelper.GetTestPost() };
            var postDtos = posts.Select(p => TestHelper.GetPostResponseDto()).ToList();
            _postServiceMock.Setup(s => s.GetTopCommentedPosts(It.IsAny<int>())).Returns(posts);
            _modelMapperMock.Setup(m => m.ToPostResponseDto(It.IsAny<Post>())).Returns(TestHelper.GetPostResponseDto());

            // Act
            var result = _controller.GetTopPosts() as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(postDtos.Count, ((IEnumerable<PostResponseDto>)result.Value).Count());
        }

        [TestMethod]
        public void GetRecentPosts_ShouldReturnOkResult_WithPostDtos()
        {
            // Arrange
            var posts = new List<Post> { TestHelper.GetTestPost() };
            var postDtos = posts.Select(p => TestHelper.GetPostResponseDto()).ToList();
            _postServiceMock.Setup(s => s.GetRecentPosts(It.IsAny<int>())).Returns(posts);
            _modelMapperMock.Setup(m => m.ToPostResponseDto(It.IsAny<Post>())).Returns(TestHelper.GetPostResponseDto());

            // Act
            var result = _controller.GetRecentPosts() as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(postDtos.Count, ((IEnumerable<PostResponseDto>)result.Value).Count());
        }

        [TestMethod]
        [DataRow(1)]
        public void GetUserPosts_ShouldReturnOkResult_WithPostDtos(int userId)
        {
            // Arrange
            var posts = new List<Post> { TestHelper.GetTestPost() };
            var postDtos = posts.Select(p => TestHelper.GetPostResponseDto()).ToList();
            _postServiceMock.Setup(s => s.GetPostsByUserId(userId)).Returns(posts);
            _modelMapperMock.Setup(m => m.ToPostResponseDto(It.IsAny<Post>())).Returns(TestHelper.GetPostResponseDto());

            // Act
            var result = _controller.GetUserPosts(userId) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(postDtos.Count, ((IEnumerable<PostResponseDto>)result.Value).Count());
        }

        [TestMethod]
        public void FilterPosts_ShouldReturnOkResult_WithPostDtos()
        {
            // Arrange
            var filterParameters = new PostQueryParameters();
            var posts = new List<Post> { TestHelper.GetTestPost() };
            var postDtos = posts.Select(p => TestHelper.GetPostResponseDto()).ToList();
            _postServiceMock.Setup(s => s.FilterBy(filterParameters)).Returns(posts);
            _modelMapperMock.Setup(m => m.ToPostResponseDto(It.IsAny<Post>())).Returns(TestHelper.GetPostResponseDto());

            // Act
            var result = _controller.FilterPosts(filterParameters) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(postDtos.Count, ((IEnumerable<PostResponseDto>)result.Value).Count());
        }
    }
}
