using ForumProject.Data;
using ForumProject.Models;
using ForumProject.Models.QueryParameters;
using ForumProject.Repositories;
using ForumProject.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using ForumProject.Models.Enums;

namespace ForumProject.Tests.Repositories
{
    [TestClass]
    public class CommentRepositoryTests
    {
        private ApplicationDbContext _dbContext;
        private CommentRepository _commentRepository;

        [TestInitialize]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .EnableSensitiveDataLogging()
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _commentRepository = new CommentRepository(_dbContext);

            // Seed the database with test data
            SeedData();
        }

        private void SeedData()
        {
            var users = new List<User>
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
            };

            var posts = new List<Post>
            {
                new Post
                {
                    Id = 1,
                    Title = "Test Post 1",
                    Content = "Content for Test Post 1",
                    UserId = 1,
                    Category = PostCategory.Story
                },
                new Post
                {
                    Id = 2,
                    Title = "Test Post 2",
                    Content = "Content for Test Post 2",
                    UserId = 2,
                    Category = PostCategory.Story
                }
            };

            var comments = new List<Comment>
            {
                new Comment
                {
                    Id = 1,
                    Content = "Test Comment 1",
                    PostId = 1,
                    UserId = 1
                },
                new Comment
                {
                    Id = 2,
                    Content = "Test Comment 2",
                    PostId = 1,
                    UserId = 2
                },
                new Comment
                {
                    Id = 3,
                    Content = "Test Comment 3",
                    PostId = 2,
                    UserId = 1
                },
                new Comment
                {
                    Id = 4,
                    Content = "Test Comment 4",
                    PostId = 2,
                    UserId = 2,
                    ParentCommentId = 3
                }
            };

            _dbContext.Users.AddRange(users);
            _dbContext.Posts.AddRange(posts);
            _dbContext.Comments.AddRange(comments);
            _dbContext.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [TestMethod]
        public void AddComment_Should_AddNewComment()
        {
            // Arrange
            var newComment = new Comment
            {
                Content = "New Test Comment",
                PostId = 1,
                UserId = 1
            };

            // Act
            _commentRepository.AddComment(newComment);

            // Assert
            var comment = _dbContext.Comments.FirstOrDefault(c => c.Content == "New Test Comment");
            Assert.IsNotNull(comment);
            Assert.AreEqual(newComment.Content, comment.Content);
            Assert.AreEqual(newComment.PostId, comment.PostId);
            Assert.AreEqual(newComment.UserId, comment.UserId);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException))]
        public void DeleteComment_Should_ThrowException_When_CommentDoesNotExist()
        {
            // Arrange
            var commentId = 99;

            // Act
            _commentRepository.DeleteComment(commentId);

            // Assert - [ExpectedException] handles this
        }

        [TestMethod]
        public void DeleteComment_Should_SoftDeleteComment_When_CommentExists()
        {
            // Arrange
            var commentId = 1;

            // Act
            _commentRepository.DeleteComment(commentId);

            // Assert
            var comment = _dbContext.Comments.FirstOrDefault(c => c.Id == commentId);
            Assert.IsNotNull(comment);
            Assert.IsNotNull(comment.DeletedAt);
        }

        [TestMethod]
        public void GetCommentById_Should_ReturnComment_When_CommentExists()
        {
            // Arrange
            var commentId = 1;

            // Act
            var result = _commentRepository.GetCommentById(commentId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(commentId, result.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException))]
        public void GetCommentById_Should_ThrowException_When_CommentDoesNotExist()
        {
            // Arrange
            var commentId = 99;

            // Act
            _commentRepository.GetCommentById(commentId);

            // Assert - [ExpectedException] handles this
        }

        [TestMethod]
        public void GetCommentsByPostId_Should_ReturnComments_When_PostIdIsValid()
        {
            // Arrange
            var postId = 1;

            // Act
            var results = _commentRepository.GetCommentsByPostId(postId);

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(2, results.Count()); 
        }

        [TestMethod]
        public void GetRepliesByCommentId_Should_ReturnReplies_When_CommentIdIsValid()
        {
            // Arrange
            var commentId = 3;

            // Act
            var results = _commentRepository.GetRepliesByCommentId(commentId);

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count());
        }

        [TestMethod]
        public void UpdateComment_Should_UpdateComment_When_ItExists()
        {
            // Arrange
            var updatedComment = new Comment
            {
                Id = 1,
                Content = "Updated Test Comment",
                PostId = 1,
                UserId = 1
            };

            // Act
            _commentRepository.UpdateComment(updatedComment);

            // Assert
            var comment = _dbContext.Comments.FirstOrDefault(c => c.Id == updatedComment.Id);
            Assert.IsNotNull(comment);
            Assert.AreEqual(updatedComment.Content, comment.Content);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException))]
        public void UpdateComment_Should_ThrowException_When_CommentDoesNotExist()
        {
            // Arrange
            var updatedComment = new Comment
            {
                Id = 99,
                Content = "Updated Test Comment",
                PostId = 1,
                UserId = 1
            };

            // Act
            _commentRepository.UpdateComment(updatedComment);

            // Assert - [ExpectedException] handles this
        }

        //[TestMethod]
        //public void FilterBy_Should_ReturnComments_When_FilterParametersMatch()
        //{
        //    // Arrange
        //    var filterParameters = new CommentQueryParameters
        //    {
        //        PostId = 1,
        //        UserId = 1,
        //        CreatedAfter = DateTime.Now.AddDays(-1),
        //        CreatedBefore = DateTime.Now.AddDays(1)
        //    };

        //    // Act
        //    var results = _commentRepository.FilterBy(filterParameters);

        //    // Assert
        //    Assert.IsNotNull(results);
        //    Assert.AreEqual(1, results.Count());
        //}
    }
}
