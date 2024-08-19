using ForumProject.Models;
using ForumProject.Models.Enums;
using ForumProject.Models.QueryParameters;
using ForumProject.Repositories.Contracts;
using ForumProject.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace ForumProject.Tests.Services.PostServiceTests
{
    [TestClass]
    public class PostServiceTests
    {
        private Mock<IPostRepository> _postRepositoryMock;
        private PostService _postService;

        [TestInitialize]
        public void SetUp()
        {
            _postRepositoryMock = new Mock<IPostRepository>();
            _postService = new PostService(_postRepositoryMock.Object);
        }

        [TestMethod]
        public void GetPostById_ShouldReturnPost_WhenPostExists()
        {
            // Arrange
            var postId = 1;
            var expectedPost = TestHelper.GetTestPost();
            _postRepositoryMock.Setup(x => x.GetPostById(postId)).Returns(expectedPost);

            // Act
            var result = _postService.GetPostById(postId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedPost, result);
        }

        [TestMethod]
        public void GetAllPosts_ShouldReturnAllPosts()
        {
            // Arrange
            var expectedPosts = new List<Post> { TestHelper.GetTestPost() }.AsQueryable();
            _postRepositoryMock.Setup(x => x.GetAllPosts()).Returns(expectedPosts);

            // Act
            var result = _postService.GetAllPosts();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedPosts.Count(), result.Count());
        }

        [TestMethod]
        public void GetTopCommentedPosts_ShouldReturnTopCommentedPosts()
        {
            // Arrange
            var count = 1;
            var expectedPosts = new List<Post> { TestHelper.GetTestPost() }.AsQueryable();
            _postRepositoryMock.Setup(x => x.GetTopCommentedPosts(count)).Returns(expectedPosts);

            // Act
            var result = _postService.GetTopCommentedPosts(count);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedPosts.Count(), result.Count());
        }

        [TestMethod]
        public void GetTopReactedPosts_ShouldReturnTopReactedPosts()
        {
            // Arrange
            var count = 1;
            var expectedPosts = new List<Post> { TestHelper.GetTestPost() }.AsQueryable();
            _postRepositoryMock.Setup(x => x.GetTopReactedPosts(count)).Returns(expectedPosts);

            // Act
            var result = _postService.GetTopReactedPosts(count);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedPosts.Count(), result.Count());
        }

        [TestMethod]
        public void GetRecentPosts_ShouldReturnRecentPosts()
        {
            // Arrange
            var count = 1;
            var expectedPosts = new List<Post> { TestHelper.GetTestPost() }.AsQueryable();
            _postRepositoryMock.Setup(x => x.GetRecentPosts(count)).Returns(expectedPosts);

            // Act
            var result = _postService.GetRecentPosts(count);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedPosts.Count(), result.Count());
        }

        [TestMethod]
        public void AddPost_ShouldCallAddPostOnRepository()
        {
            // Arrange
            var newPost = TestHelper.GetTestPost();

            // Act
            _postService.AddPost(newPost);

            // Assert
            _postRepositoryMock.Verify(x => x.AddPost(newPost), Times.Once);
        }

        [TestMethod]
        public void UpdatePost_ShouldCallUpdatePostOnRepository()
        {
            // Arrange
            var postToUpdate = TestHelper.GetTestPost();

            // Act
            _postService.UpdatePost(postToUpdate);

            // Assert
            _postRepositoryMock.Verify(x => x.UpdatePost(postToUpdate), Times.Once);
        }

        [TestMethod]
        public void DeletePost_ShouldCallDeletePostOnRepository()
        {
            // Arrange
            var postId = 1;

            // Act
            _postService.DeletePost(postId);

            // Assert
            _postRepositoryMock.Verify(x => x.DeletePost(postId), Times.Once);
        }

        [TestMethod]
        public void GetPostsByUserId_ShouldReturnPostsByUserId()
        {
            // Arrange
            var userId = 1;
            var expectedPosts = new List<Post> { TestHelper.GetTestPost() }.AsQueryable();
            _postRepositoryMock.Setup(x => x.GetPostsByUserId(userId)).Returns(expectedPosts);

            // Act
            var result = _postService.GetPostsByUserId(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedPosts.Count(), result.Count());
        }

        [TestMethod]
        public void FilterBy_ShouldReturnFilteredPosts()
        {
            // Arrange
            var filterParameters = new PostQueryParameters();
            var expectedPosts = new List<Post> { TestHelper.GetTestPost() }.AsQueryable();
            _postRepositoryMock.Setup(x => x.FilterBy(filterParameters)).Returns(expectedPosts);

            // Act
            var result = _postService.FilterBy(filterParameters);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedPosts.Count(), result.Count());
        }

        [TestMethod]
        public void GetTotalCount_ShouldReturnTotalCount()
        {
            // Arrange
            var filterParameters = new PostQueryParameters();
            var expectedCount = 1;
            _postRepositoryMock.Setup(x => x.GetTotalCount(filterParameters)).Returns(expectedCount);

            // Act
            var result = _postService.GetTotalCount(filterParameters);

            // Assert
            Assert.AreEqual(expectedCount, result);
        }
    }
}
