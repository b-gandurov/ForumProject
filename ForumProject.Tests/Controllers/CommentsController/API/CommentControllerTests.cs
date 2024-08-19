using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ForumProject.Controllers.API;
using ForumProject.Models.DTOs;
using ForumProject.Services.Contracts;
using ForumProject.Models;
using System.Collections.Generic;
using System.Linq;
using ForumProject.Helpers;

namespace ForumProject.Tests.Controllers.CommentsController.API
{
    [TestClass]
    public class CommentControllerTests
    {
        private Mock<ICommentService> _commentServiceMock;
        private Mock<IModelMapper> _modelMapperMock;
        private Mock<IAuthService> _authServiceMock;
        private CommentController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _commentServiceMock = new Mock<ICommentService>();
            _modelMapperMock = new Mock<IModelMapper>();
            _authServiceMock = new Mock<IAuthService>();
            _controller = new CommentController(_commentServiceMock.Object, _modelMapperMock.Object, _authServiceMock.Object);
        }

        [TestMethod]
        public void GetCommentById_ShouldReturnComment_WhenCommentExists()
        {
            // Arrange
            var commentId = 1;
            var comment = TestHelper.GetTestComment();
            var commentDto = TestHelper.GetCommentResponseDto();
            _commentServiceMock.Setup(x => x.GetCommentById(commentId)).Returns(comment);
            _modelMapperMock.Setup(x => x.ToCommentResponseDto(comment)).Returns(commentDto);

            // Act
            var result = _controller.GetCommentById(commentId) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(commentDto, result.Value);
        }

        [TestMethod]
        public void GetCommentsByPostId_ShouldReturnComments_WhenCommentsExist()
        {
            // Arrange
            var postId = 1;
            var comments = new List<Comment> { TestHelper.GetTestComment() }.AsQueryable();
            var commentDtos = new List<CommentResponseDto> { TestHelper.GetCommentResponseDto() };
            _commentServiceMock.Setup(x => x.GetCommentsByPostId(postId)).Returns(comments);
            _modelMapperMock.Setup(x => x.ToCommentResponseDto(It.IsAny<Comment>())).Returns(commentDtos.First());

            // Act
            var result = _controller.GetCommentsByPostId(postId) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

        }

        [TestMethod]
        public void GetRepliesByCommentId_ShouldReturnReplies_WhenRepliesExist()
        {
            // Arrange
            var commentId = 1;
            var replies = new List<Comment> { TestHelper.GetTestReply() }.AsQueryable();
            var replyDtos = new List<CommentResponseDto> { TestHelper.GetCommentResponseDto() };
            _commentServiceMock.Setup(x => x.GetRepliesByCommentId(commentId)).Returns(replies);
            _modelMapperMock.Setup(x => x.ToCommentResponseDto(It.IsAny<Comment>())).Returns(replyDtos.First());

            // Act
            var result = _controller.GetRepliesByCommentId(commentId) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
           
        }

        //[TestMethod]
        //public void AddComment_ShouldReturnCreatedComment_WhenValidData()
        //{
        //    // Arrange
        //    var postId = 1;
        //    var commentRequestDto = TestHelper.GetCommentRequestDto();
        //    var comment = TestHelper.GetTestComment();
        //    var commentResponseDto = TestHelper.GetCommentResponseDto();
        //    var user = TestHelper.GetTestUser();
        //    _modelMapperMock.Setup(x => x.ToComment(commentRequestDto)).Returns(comment);
        //    _modelMapperMock.Setup(x => x.ToCommentResponseDto(comment)).Returns(commentResponseDto);
        //    _commentServiceMock.Setup(x => x.AddComment(comment));
        //    _controller.ControllerContext.HttpContext.Items["User"] = user;

        //    // Act
        //    var result = _controller.AddComment(postId, commentRequestDto) as CreatedAtActionResult;

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(201, result.StatusCode);
        //    Assert.AreEqual(commentResponseDto, result.Value);
        //}

        //[TestMethod]
        //public void AddReply_ShouldReturnCreatedReply_WhenValidData()
        //{
        //    // Arrange
        //    var postId = 1;
        //    var parrentCommentId = 1;
        //    var commentRequestDto = TestHelper.GetCommentRequestDto();
        //    var reply = TestHelper.GetTestReply();
        //    var replyResponseDto = TestHelper.GetCommentResponseDto();
        //    var user = TestHelper.GetTestUser();
        //    _modelMapperMock.Setup(x => x.ToComment(commentRequestDto)).Returns(reply);
        //    _modelMapperMock.Setup(x => x.ToCommentResponseDto(reply)).Returns(replyResponseDto);
        //    _commentServiceMock.Setup(x => x.AddReply(reply));
        //    _controller.ControllerContext.HttpContext.Items["User"] = user;

        //    // Act
        //    var result = _controller.AddReply(postId, parrentCommentId, commentRequestDto) as CreatedAtActionResult;

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(201, result.StatusCode);
        //    Assert.AreEqual(replyResponseDto, result.Value);
        //}

        //[TestMethod]
        //public void UpdateComment_ShouldReturnUpdatedComment_WhenValidData()
        //{
        //    // Arrange
        //    var postId = 1;
        //    var commentId = 1;
        //    var commentRequestDto = TestHelper.GetCommentRequestDto();
        //    var comment = TestHelper.GetTestComment();
        //    var commentResponseDto = TestHelper.GetCommentResponseDto();
        //    var user = TestHelper.GetTestUser();
        //    _commentServiceMock.Setup(x => x.GetCommentById(commentId)).Returns(comment);
        //    _modelMapperMock.Setup(x => x.ToCommentResponseDto(comment)).Returns(commentResponseDto);
        //    _commentServiceMock.Setup(x => x.UpdateComment(comment));
        //    _controller.ControllerContext.HttpContext.Items["User"] = user;

        //    // Act
        //    var result = _controller.UpdateComment(postId, commentId, commentRequestDto) as OkObjectResult;

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(200, result.StatusCode);
            
        //}

        [TestMethod]
        public void DeleteComment_ShouldReturnNoContent_WhenCommentIsDeleted()
        {
            // Arrange
            var commentId = 1;
            var comment = TestHelper.GetTestComment();
            _commentServiceMock.Setup(x => x.GetCommentById(commentId)).Returns(comment);
            _commentServiceMock.Setup(x => x.DeleteComment(commentId));

            // Act
            var result = _controller.DeleteComment(commentId) as NoContentResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(204, result.StatusCode);
        }
    }
}
