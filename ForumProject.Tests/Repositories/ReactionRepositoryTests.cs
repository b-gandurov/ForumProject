using ForumProject.Data;
using ForumProject.Helpers;
using ForumProject.Models;
using ForumProject.Models.DTOs;
using ForumProject.Models.Enums;
using ForumProject.Models.QueryParameters;
using ForumProject.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ForumProject.Tests.Repositories
{
    [TestClass]
    public class ReactionRepositoryTests
    {
        private ApplicationDbContext _dbContext;
        private ReactionRepository _reactionRepository;
        private IModelMapper _modelMapper;

        [TestInitialize]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .EnableSensitiveDataLogging()
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _modelMapper = new ModelMapper();
            _reactionRepository = new ReactionRepository(_dbContext, _modelMapper);

            // Seed the database with test data
            _dbContext.Reactions.AddRange(new List<Reaction>
            {
                new Reaction
                {
                    Id = 1,
                    UserId = 1,
                    ReactionType = ReactionType.Like,
                    ReactionTarget = new ReactionTarget
                    {
                        PostId = 1,
                        CommentId = null
                    },
                    CreatedDate = DateTime.Now
                },
                new Reaction
                {
                    Id = 2,
                    UserId = 2,
                    ReactionType = ReactionType.Dislike,
                    ReactionTarget = new ReactionTarget
                    {
                        PostId = 2,
                        CommentId = null
                    },
                    CreatedDate = DateTime.Now
                }
            });
            _dbContext.Users.AddRange(new List<User>
            {
                new User
                {
                    Id = 1,
                    Username = "testuser1",
                    Password = "Password1!", 
                    Email = "test1@example.com",
                    FirstName = "First1",
                    LastName = "Last1"
                },
                new User
                {
                    Id = 2,
                    Username = "testuser2",
                    Password = "Password2!",
                    Email = "test2@example.com",
                    FirstName = "First2",
                    LastName = "Last2"
                }
            });
            _dbContext.Posts.AddRange(new List<Post>
            {
                new Post
                {
                    Id = 1,
                    Title = "Title",
                    Content = "Content!",
                    UserId = 1,
                    Category = PostCategory.Story,
                }
            });
            _dbContext.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [TestMethod]
        public void AddReaction_Should_UpdateReaction_When_ItAlreadyExists()
        {
            // Arrange
            var reactionDto = new ReactionRequestDto
            {
                Id = 1,
                UserId = 1,
                ReactionType = ReactionType.Dislike,
                PostId = 1
            };

            // Act
            _reactionRepository.AddReaction(reactionDto);

            // Assert
            var reaction = _dbContext.Reactions.FirstOrDefault(r => r.Id == reactionDto.Id);
            Assert.IsNotNull(reaction);
            Assert.AreEqual(reactionDto.ReactionType, reaction.ReactionType);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void DeleteReaction_Should_ThrowException_When_ReactionDoesNotExist()
        {
            // Arrange
            var reactionId = 99;

            // Act
            _reactionRepository.DeleteReaction(reactionId);

            // Assert - [ExpectedException] handles this
        }

        [TestMethod]
        public void FilterBy_Should_ReturnReactions_When_FilterParametersMatch()
        {
            // Arrange
            var filterParameters = new ReactionQueryParameters
            {
                PostId = 1,
                UserId = 1,
                Type = ReactionType.Like
            };

            // Act
            var results = _reactionRepository.FilterBy(filterParameters);

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count());
        }

        [TestMethod]
        public void GetReactionById_Should_ReturnReaction_When_ReactionExists()
        {
            // Arrange
            var reactionId = 1;

            // Act
            var result = _reactionRepository.GetReactionById(reactionId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(reactionId, result.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetReactionById_Should_ThrowException_When_ReactionDoesNotExist()
        {
            // Arrange
            var reactionId = 99;

            // Act
            _reactionRepository.GetReactionById(reactionId);

            // Assert - [ExpectedException] handles this
        }

        [TestMethod]
        public void GetReactionsByPostId_Should_ReturnReactions_When_PostIdIsValid()
        {
            // Arrange
            var postId = 1;

            // Act
            var results = _reactionRepository.GetReactionsByPostId(postId);

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count());
        }

        [TestMethod]
        public void UpdateReaction_Should_UpdateReaction_When_ItExists()
        {
            // Arrange
            var reactionDto = new ReactionRequestDto
            {
                Id = 1,
                ReactionType = ReactionType.Dislike,
            };

            // Act
            _reactionRepository.UpdateReaction(reactionDto);

            // Assert
            var reaction = _dbContext.Reactions.FirstOrDefault(r => r.Id == reactionDto.Id);
            Assert.IsNotNull(reaction);
            Assert.AreEqual(reactionDto.ReactionType, reaction.ReactionType);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void UpdateReaction_Should_ThrowException_When_ReactionDoesNotExist()
        {
            // Arrange
            var reactionDto = new ReactionRequestDto
            {
                Id = 99, // Non-existing ID
                ReactionType = ReactionType.Like
            };

            // Act
            _reactionRepository.UpdateReaction(reactionDto);

            // Assert - [ExpectedException] handles this
        }
    }
}
