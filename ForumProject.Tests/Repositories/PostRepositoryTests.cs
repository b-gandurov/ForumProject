using ForumProject.Data;
using ForumProject.Exceptions;
using ForumProject.Models;
using ForumProject.Models.Enums;
using ForumProject.Models.QueryParameters;
using ForumProject.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ForumProject.Tests.Repositories
{
    [TestClass]
    public class PostRepositoryTests
    {
        private ApplicationDbContext _dbContext;
        private PostRepository _postRepository;

        [TestInitialize]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .EnableSensitiveDataLogging()
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _postRepository = new PostRepository(_dbContext);

            
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
                    Category = PostCategory.Story,
                },
                new Post
                {
                    Id = 2,
                    Title = "Test Post 2",
                    Content = "Content for Test Post 2",
                    UserId = 2,
                    Category = PostCategory.Story,
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
        public void AddPost_Should_AddNewPost()
        {
            // Arrange
            var newPost = new Post
            {
                Title = "New Test Post",
                Content = "Content for New Test Post",
                UserId = 1,
                Category = PostCategory.Story,
            };

            // Act
            _postRepository.AddPost(newPost);

            // Assert
            var post = _dbContext.Posts.FirstOrDefault(p => p.Title == "New Test Post");
            Assert.IsNotNull(post);
            Assert.AreEqual(newPost.Title, post.Title);
            Assert.AreEqual(newPost.Content, post.Content);
            Assert.AreEqual(newPost.UserId, post.UserId);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException))]
        public void DeletePost_Should_ThrowException_When_PostDoesNotExist()
        {
            // Arrange
            var postId = 99;

            // Act
            _postRepository.DeletePost(postId);

            // Assert - [ExpectedException] handles this
        }

        //[TestMethod]
        //public void DeletePost_Should_DeletePost_When_PostExists()
        //{
        //    // Arrange
        //    var postId = 1;

        //    // Act
        //    _postRepository.DeletePost(postId);

        //    // Assert
        //    var post = _dbContext.Posts.FirstOrDefault(p => p.Id == postId);
        //    Assert.IsNull(post);
        //}

        [TestMethod]
        public void GetPostById_Should_ReturnPost_When_PostExists()
        {
            // Arrange
            var postId = 1;

            // Act
            var result = _postRepository.GetPostById(postId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(postId, result.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException))]
        public void GetPostById_Should_ThrowException_When_PostDoesNotExist()
        {
            // Arrange
            var postId = 99;

            // Act
            _postRepository.GetPostById(postId);

            // Assert - [ExpectedException] handles this
        }

        [TestMethod]
        public void GetAllPosts_Should_ReturnAllPosts()
        {
            // Act
            var results = _postRepository.GetAllPosts();

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(2, results.Count()); 
        }

        [TestMethod]
        public void GetTopCommentedPosts_Should_ReturnTopCommentedPosts()
        {
            // Arrange
            var count = 1;

            // Act
            var results = _postRepository.GetTopCommentedPosts(count);

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(count, results.Count());
        }

        [TestMethod]
        public void GetRecentPosts_Should_ReturnRecentPosts()
        {
            // Arrange
            var count = 1;

            // Act
            var results = _postRepository.GetRecentPosts(count);

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(count, results.Count());
        }

        [TestMethod]
        public void UpdatePost_Should_UpdatePost_When_ItExists()
        {
            // Arrange
            var updatedPost = new Post
            {
                Id = 1,
                Title = "Updated Test Post",
                Content = "Updated Content for Test Post",
                UserId = 1,
                Category = PostCategory.Story,
            };

            // Act
            _postRepository.UpdatePost(updatedPost);

            // Assert
            var post = _dbContext.Posts.FirstOrDefault(p => p.Id == updatedPost.Id);
            Assert.IsNotNull(post);
            Assert.AreEqual(updatedPost.Title, post.Title);
            Assert.AreEqual(updatedPost.Content, post.Content);
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException))]
        public void UpdatePost_Should_ThrowException_When_PostDoesNotExist()
        {
            // Arrange
            var updatedPost = new Post
            {
                Id = 99,
                Title = "Updated Test Post",
                Content = "Updated Content for Test Post",
                UserId = 1,
                Category = PostCategory.Story,
            };

            // Act
            _postRepository.UpdatePost(updatedPost);

            // Assert - [ExpectedException] handles this
        }

        [TestMethod]
        public void GetPostsByUserId_Should_ReturnPosts_When_UserIdIsValid()
        {
            // Arrange
            var userId = 1;

            // Act
            var results = _postRepository.GetPostsByUserId(userId);

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException))]
        public void GetPostsByUserId_Should_ThrowException_When_UserDoesNotExist()
        {
            // Arrange
            var userId = 99;

            // Act
            _postRepository.GetPostsByUserId(userId);

            // Assert - [ExpectedException] handles this
        }

        [TestMethod]
        public void FilterBy_Should_ReturnPosts_When_FilterParametersMatch()
        {
            // Arrange
            var filterParameters = new PostQueryParameters
            {
                Title = "Test",
                CreatedAfter = DateTime.Now.AddDays(-1),
                CreatedBefore = DateTime.Now.AddDays(1),
                UserName = "testuser1",
                Category = PostCategory.Story,
                PageNumber = 1,
                PageSize = 10
            };

            // Act
            var results = _postRepository.FilterBy(filterParameters);

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count());
        }
    }
}
