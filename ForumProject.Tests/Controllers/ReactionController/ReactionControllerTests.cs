using ForumProject.Helpers;
using ForumProject.Models;
using ForumProject.Models.DTOs;
using ForumProject.Models.Enums;
using ForumProject.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ForumProject.Tests.Controllers.ReactionController
{
    [TestClass]
    public class ReactionControllerTests
    {
        private Mock<IModelMapper> _mockModelMapper;
        private Mock<IReactionService> _mockReactionService;
        private Mock<IUserService> _mockUserService;
        private Mock<IUserContextService> _mockUserContextService;
        private ForumProject.Controllers.MVC.ReactionController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _mockModelMapper = new Mock<IModelMapper>();
            _mockReactionService = new Mock<IReactionService>();
            _mockUserService = new Mock<IUserService>();
            _mockUserContextService = new Mock<IUserContextService>();

            _controller = new ForumProject.Controllers.MVC.ReactionController(
                _mockModelMapper.Object,
                _mockReactionService.Object,
                _mockUserService.Object,
                _mockUserContextService.Object
            );
        }

        [TestMethod]
        public void AddReaction_ShouldRedirectToPostDetails_WhenValidInput()
        {
            // Arrange
            var reactionType = (int)ReactionType.Like; // Assume ReactionType is an enum
            var postId = 1;
            var commentId = (int?)null;
            var currentUser = new User { Username = "testuser" };
            var user = new User { Id = 1 };

            _mockUserContextService.Setup(s => s.GetCurrentUser()).Returns(currentUser);
            _mockUserService.Setup(s => s.GetUserByUsername(currentUser.Username)).Returns(user);

            // Act
            var result = _controller.AddReaction(reactionType, postId, commentId) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Details", result.ActionName);
            Assert.AreEqual("Posts", result.ControllerName);
            Assert.AreEqual(postId, result.RouteValues["id"]);
        }

        [TestMethod]
        public void AddReaction_ShouldReturnNotFound_WhenExceptionOccurs()
        {
            // Arrange
            var reactionType = (int)ReactionType.Like; // Assume ReactionType is an enum
            var postId = 1;
            var commentId = (int?)null;
            var currentUser = new User { Username = "testuser" };

            _mockUserContextService.Setup(s => s.GetCurrentUser()).Returns(currentUser);
            _mockUserService.Setup(s => s.GetUserByUsername(currentUser.Username)).Throws(new Exception());

            // Act
            var result = _controller.AddReaction(reactionType, postId, commentId) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddReaction_ShouldCallAddReactionServiceMethod_WhenValidInput()
        {
            // Arrange
            var reactionType = (int)ReactionType.Like; // Assume ReactionType is an enum
            var postId = 1;
            var commentId = (int?)null;
            var currentUser = new User { Username = "testuser" };
            var user = new User { Id = 1 };

            _mockUserContextService.Setup(s => s.GetCurrentUser()).Returns(currentUser);
            _mockUserService.Setup(s => s.GetUserByUsername(currentUser.Username)).Returns(user);

            // Act
            _controller.AddReaction(reactionType, postId, commentId);

            // Assert
            _mockReactionService.Verify(s => s.AddReaction(It.Is<ReactionRequestDto>(r =>
                r.ReactionType == (ReactionType)reactionType &&
                r.PostId == postId &&
                r.CommentId == commentId &&
                r.UserId == user.Id
            )), Times.Once);
        }
    }
}
