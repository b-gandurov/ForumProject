using ForumProject.Models;
using ForumProject.Repositories.Contracts;
using ForumProject.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace ForumProject.Tests.Services.CommentServiceTests

{
    [TestClass]
    public class CommentServiceTests
    {
        private Mock<ICommentRepository> _commentRepositoryMock;
        private CommentService _commentService;

        [TestInitialize]
        public void SetUp()
        {
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _commentService = new CommentService(_commentRepositoryMock.Object);
        }

        [TestMethod]
        public void GetCommentById_Should_ReturnComment_When_CommentExists()
        {
            // Arrange
            var commentId = 1;
            var expectedComment = TestHelper.GetTestComment();
            _commentRepositoryMock.Setup(x => x.GetCommentById(commentId)).Returns(expectedComment);

            // Act
            var result = _commentService.GetCommentById(commentId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedComment, result);
        }

        [TestMethod]
        public void GetCommentsByPostId_Should_ReturnComments_When_CommentsExist()
        {
            // Arrange
            var postId = 1;
            var expectedComments = new List<Comment> { TestHelper.GetTestComment() }.AsQueryable();
            _commentRepositoryMock.Setup(x => x.GetCommentsByPostId(postId)).Returns(expectedComments);

            // Act
            var result = _commentService.GetCommentsByPostId(postId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedComments.Count(), result.Count());
            Assert.AreEqual(expectedComments, result);
        }

        [TestMethod]
        public void GetRepliesByCommentId_Should_ReturnReplies_When_RepliesExist()
        {
            // Arrange
            var commentId = 1;
            var expectedReplies = new List<Comment> { TestHelper.GetTestReply() }.AsQueryable();
            _commentRepositoryMock.Setup(x => x.GetRepliesByCommentId(commentId)).Returns(expectedReplies);

            // Act
            var result = _commentService.GetRepliesByCommentId(commentId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedReplies.Count(), result.Count());
            Assert.AreEqual(expectedReplies, result);
        }

        [TestMethod]
        public void AddComment_Should_CallRepositoryMethod()
        {
            // Arrange
            var comment = TestHelper.GetTestComment();

            // Act
            _commentService.AddComment(comment);

            // Assert
            _commentRepositoryMock.Verify(x => x.AddComment(comment), Times.Once);
        }

        [TestMethod]
        public void AddReply_Should_CallRepositoryMethod()
        {
            // Arrange
            var reply = TestHelper.GetTestReply();

            // Act
            _commentService.AddReply(reply);

            // Assert
            _commentRepositoryMock.Verify(x => x.AddComment(reply), Times.Once);
        }

        [TestMethod]
        public void UpdateComment_Should_CallRepositoryMethod()
        {
            // Arrange
            var comment = TestHelper.GetTestComment();

            // Act
            _commentService.UpdateComment(comment);

            // Assert
            _commentRepositoryMock.Verify(x => x.UpdateComment(comment), Times.Once);
        }

        [TestMethod]
        public void DeleteComment_Should_CallRepositoryMethod()
        {
            // Arrange
            var commentId = 1;

            // Act
            _commentService.DeleteComment(commentId);

            // Assert
            _commentRepositoryMock.Verify(x => x.DeleteComment(commentId), Times.Once);
        }
    }
}
