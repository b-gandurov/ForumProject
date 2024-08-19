using ForumProject.Models.DTOs;
using ForumProject.Repositories.Contracts;
using ForumProject.Services;
using Moq;

namespace ForumProject.Tests.Services.ReactionServiceTests
{
    [TestClass]
    public class ReactionServiceTests
    {
        private Mock<IReactionRepository> _reactionRepositoryMock;
        private ReactionService _reactionService;

        [TestInitialize]
        public void SetUp()
        {
            _reactionRepositoryMock = new Mock<IReactionRepository>();
            _reactionService = new ReactionService(_reactionRepositoryMock.Object);
        }

        [TestMethod]
        public void AddReaction_Should_CallRepositoryMethod()
        {
            // Arrange
            var reactionDto = TestHelper.GetReactionRequestDto();

            // Act
            _reactionService.AddReaction(reactionDto);

            // Assert
            _reactionRepositoryMock.Verify(x => x.AddReaction(reactionDto), Times.Once);
        }

        [TestMethod]
        public void DeleteReaction_Should_CallRepositoryMethod()
        {
            // Arrange
            var reactionId = 1;

            // Act
            _reactionService.DeleteReaction(reactionId);

            // Assert
            _reactionRepositoryMock.Verify(x => x.DeleteReaction(reactionId), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteReaction_Should_ThrowArgumentException_When_ReactionNotFound()
        {
            // Arrange
            var reactionId = 1;
            _reactionRepositoryMock.Setup(x => x.DeleteReaction(reactionId)).Throws(new ArgumentException());

            // Act
            _reactionService.DeleteReaction(reactionId);

            // Assert - [ExpectedException] handles this
        }

        [TestMethod]
        public void GetReactionById_Should_ReturnReaction_When_ReactionExists()
        {
            // Arrange
            var reactionId = 1;
            var expectedReaction = TestHelper.GetTestReactionResponseDto();
            _reactionRepositoryMock.Setup(x => x.GetReactionById(reactionId)).Returns(expectedReaction);

            // Act
            var result = _reactionService.GetReactionById(reactionId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedReaction, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetReactionById_Should_ThrowArgumentException_When_ReactionNotFound()
        {
            // Arrange
            var reactionId = 1;
            _reactionRepositoryMock.Setup(x => x.GetReactionById(reactionId)).Throws(new ArgumentException());

            // Act
            _reactionService.GetReactionById(reactionId);

            // Assert - [ExpectedException] handles this
        }

        [TestMethod]
        public void GetReactionsByPostId_Should_ReturnReactions_When_ReactionsExist()
        {
            // Arrange
            var postId = 1;
            var expectedReactions = new List<ReactionResponseDto> { TestHelper.GetTestReactionResponseDto() };
            _reactionRepositoryMock.Setup(x => x.GetReactionsByPostId(postId)).Returns(expectedReactions);

            // Act
            var result = _reactionService.GetReactionsByPostId(postId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedReactions.Count, result.Count());
            CollectionAssert.AreEqual(expectedReactions, new List<ReactionResponseDto>(result));
        }

        [TestMethod]
        public void GetReactionsByCommentId_Should_ReturnReactions_When_ReactionsExist()
        {
            // Arrange
            var commentId = 1;
            var expectedReactions = new List<ReactionResponseDto> { TestHelper.GetTestReactionResponseDto() };
            _reactionRepositoryMock.Setup(x => x.GetReactionsByCommentId(commentId)).Returns(expectedReactions);

            // Act
            var result = _reactionService.GetReactionsByCommentId(commentId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedReactions.Count, result.Count());
            CollectionAssert.AreEqual(expectedReactions, new List<ReactionResponseDto>(result));
        }

        [TestMethod]
        public void UpdateReaction_Should_CallRepositoryMethod()
        {
            // Arrange
            var reactionDto = TestHelper.GetReactionRequestDto();

            // Act
            _reactionService.UpdateReaction(reactionDto);

            // Assert
            _reactionRepositoryMock.Verify(x => x.UpdateReaction(reactionDto), Times.Once);
        }
    }
}
