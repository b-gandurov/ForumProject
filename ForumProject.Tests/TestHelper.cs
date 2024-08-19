using ForumProject.Models;
using ForumProject.Models.DTOs;
using ForumProject.Models.Enums;
using ForumProject.Models.ViewModels;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace ForumProject.Tests
{
    public static class TestHelper
    {
        public static Post GetTestPost()
        {
            return new Post
            {
                Id = 1,
                Title = "Test Post",
                Content = "This is a test post",
                Category = PostCategory.Story,
                UserId = 1,
                User = GetTestUser()
            };
        }

        public static PostViewModel GetPostViewModel()
        {
            return new PostViewModel
            {
                Id = 1,
                Title = "Test Post",
                Content = "This is a test post",
                Category = PostCategory.Story,
                Username = "testuser"
            };
        }

        public static User GetTestUser()
        {
            return new User
            {
                Id = 1,
                Username = "testuser",
                Password = "password"
            };
        }


        public static Comment GetTestComment()
        {
            return new Comment
            {
                Id = 1,
                Content = "This is a test comment",
                PostId = 1,
                UserId = 1,
                User = GetTestUser()
            };
        }

        public static CommentViewModel GetCommentViewModel()
        {
            return new CommentViewModel
            {
                Id = 1,
                Content = "This is a test comment",
                PostId = 1,
                User = new UserViewModel { Username = "testuser" }
            };
        }

        public static Comment GetTestReply()
        {
            return new Comment
            {
                Id = 2,
                Content = "This is a test reply",
                PostId = 1,
                ParentCommentId = 1,
                UserId = 1,
                User = GetTestUser()
            };
        }

        public static UserRequestDto GetUserRequestDto()
        {
            return new UserRequestDto
            {
                Username = "testuser",
                Password = "password",
                Email = "testuser@example.com"
            };
        }

        public static UserResponseDto GetUserResponseDto()
        {
            return new UserResponseDto
            {
                Username = "testuser",
                Email = "testuser@example.com"
            };
        }

        public static ReactionResponseDto GetTestReactionResponseDto()
        {
            return new ReactionResponseDto()
            {
                Id = 1,
                CreatedDate = DateTime.Now,
                ReactionType = ReactionType.Dislike,
                UserId = 1,
                PostId = 1,
            };
        }
        public static ReactionRequestDto GetReactionRequestDto()
        {
            return new ReactionRequestDto()
            {
                Id = 1,
                ReactionType = ReactionType.Dislike,
                UserId = 1,
                PostId = 1,
            };
        }

        public static IConfiguration GetJwtConfiguration()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                {"Jwt:Key", "supersecretkeythatissufficientlylong!1234567890"},
                {"Jwt:Issuer", "testIssuer"},
                {"Jwt:Audience", "testAudience"},
                {"Jwt:ExpireMinutes", "60"}
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        public static PostResponseDto GetPostResponseDto()
        {
            return new PostResponseDto
            {
                Id = 1,
                Title = "Test Post",
                Content = "This is a test post",
                Category = PostCategory.Story,
                UserName = "testuser"
            };
        }

        public static PostRequestDto GetPostRequestDto()
        {
            return new PostRequestDto
            {
                Title = "Test Post",
                Content = "This is a test post",
                Category = PostCategory.Story
            };
        }

        public static CommentResponseDto GetCommentResponseDto()
        {
            return new CommentResponseDto
            {
                Id = 1,
                Content = "This is a test comment",
                PostId = 1,
                UserName = "testuser",

            };
        }

        public static CommentRequestDto GetCommentRequestDto()
        {
            return new CommentRequestDto
            {
                Content = "This is a test comment",
                PostId = 1,
                UserId = 1,
                ParentCommentId = null
            };
        }

    }
}
