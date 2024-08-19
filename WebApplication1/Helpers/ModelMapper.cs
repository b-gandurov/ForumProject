using ForumProject.Helpers;
using ForumProject.Models.DTOs;
using ForumProject.Models;
using ForumProject.Models.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using ForumProject.Models.Enums;

public class ModelMapper : IModelMapper
{
    public ModelMapper()
    {
    }

    public CommentResponseDto ToCommentResponseDto(Comment comment)
    {
        return new CommentResponseDto
        {
            Id = comment.Id,
            Content = comment.Content,
            PostId = comment.PostId,
            ParentCommentId = comment.ParentCommentId,
            UserName = comment.User.Username,
            CreatedDate = comment.CreatedDate,
            Replies = comment.Replies?.Select(ToCommentResponseDto).ToList() ?? new List<CommentResponseDto>(),
            Reactions = comment.ReactionTargets.SelectMany(rt => rt.Reactions).Select(ToReactionResponseDto).ToList() ?? new List<ReactionResponseDto>()
        };
    }

    public Comment ToComment(CommentRequestDto commentRequestDto)
    {
        return new Comment
        {
            Content = commentRequestDto.Content,
            PostId = commentRequestDto.PostId,
            ParentCommentId = commentRequestDto.ParentCommentId,
            UserId = commentRequestDto.UserId,
        };
    }

    public Comment ToComment(CommentViewModel commentViewModel)
    {
        return new Comment
        {
            Id = commentViewModel.Id,
            Content = commentViewModel.Content,
            PostId = commentViewModel.PostId,
            ParentCommentId = commentViewModel.ParentCommentId,
            UserId = commentViewModel.User.Id,
            CreatedDate = commentViewModel.CreatedDate,
            DeletedAt = commentViewModel.DeletedAt
        };
    }

    public CommentViewModel ToCommentViewModel(Comment comment)
    {
        return new CommentViewModel
        {
            Id = comment.Id,
            Content = comment.Content,
            PostId = comment.PostId,
            ParentCommentId = comment.ParentCommentId,
            User = ToUserViewModel(comment.User),
            CreatedDate = comment.CreatedDate,
            DeletedAt = comment.DeletedAt,
            Reactions = comment.ReactionTargets.SelectMany(rt => rt.Reactions).Select(ToReactionViewModel).ToList() ?? new List<ReactionViewModel>(),
            Replies = comment.Replies?.Select(ToCommentViewModel).ToList() ?? new List<CommentViewModel>()
        };
    }

    public PostResponseDto ToPostResponseDto(Post post)
    {
        var postDto = new PostResponseDto
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            UserName = post.User.Username,
            ReactionIds = post.ReactionTargets.SelectMany(rt => rt.Reactions).Select(r => r.Id).ToList() ?? new List<int>(),
            Comments = post.Comments?.Select(c => ToCommentResponseDto(c)).ToList() ?? new List<CommentResponseDto>(),
            ImageUrl = post.ImageUrl,
            Category = post.Category,
        };
        return postDto;
    }

    public Post ToPost(PostRequestDto postDto)
    {
        return new Post
        {
            Title = postDto.Title,
            Content = postDto.Content,
            ImageUrl = postDto.ImageUrl,
            Category = postDto.Category,
        };
    }

    public PostViewModel ToPostViewModel(Post post)
    {
        return new PostViewModel
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            Username = post.User.Username,
            Comments = post.Comments?.Select(ToCommentView).ToList() ?? new List<CommentViewModel>(),
            Reactions = post.ReactionTargets.SelectMany(rt => rt.Reactions).Select(ToReactionViewModel).ToList() ?? new List<ReactionViewModel>(),
            ImageUrl = post.ImageUrl,
            Category = post.Category,
        };
    }

    public UserViewModel ToUserViewModel(User user)
    {
        return new UserViewModel
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ProfilePictureURL = user.ProfilePictureUrl
        };
    }

    public CommentViewModel ToCommentView(Comment comment)
    {
        return new CommentViewModel
        {
            Id = comment.Id,
            Content = comment.Content,
            PostId = comment.PostId,
            ParentCommentId = comment.ParentCommentId,
            User = ToUserViewModel(comment.User),
            CreatedDate = comment.CreatedDate,
            DeletedAt = comment.DeletedAt,
            Reactions = comment.ReactionTargets.SelectMany(rt => rt.Reactions).Select(ToReactionViewModel).ToList() ?? new List<ReactionViewModel>(),
            Replies = comment.Replies?.Select(ToCommentView).ToList() ?? new List<CommentViewModel>()
        };
    }

    public ReplyViewModel ToReplyViewModel(Comment reply)
    {
        return new ReplyViewModel
        {
            Id = reply.Id,
            Content = reply.Content,
            PostId = reply.PostId,
            ParentCommentId = reply.ParentCommentId,
            UserName = reply.User.Username,
            UserId = reply.UserId,
            CreatedDate = reply.CreatedDate,
            DeletedAt = reply.DeletedAt,
            Reactions = reply.ReactionTargets.SelectMany(rt => rt.Reactions).Select(ToReactionViewModel).ToList() ?? new List<ReactionViewModel>()
        };
    }

    public ReactionViewModel ToReactionViewModel(Reaction reaction)
    {
        return new ReactionViewModel
        {
            Id = reaction.Id,
            ReactionType = reaction.ReactionType.ToString(),
            PostId = reaction.ReactionTarget?.PostId,
            CommentId = reaction.ReactionTarget?.CommentId,
            UserName = reaction.User.Username
        };
    }

    public Post ToPost(PostViewModel postViewModel)
    {
        return new Post
        {
            Id = postViewModel.Id,
            Title = postViewModel.Title,
            Content = postViewModel.Content,
            UserId = postViewModel.UserId,
            ImageUrl = postViewModel.ImageUrl,
            Category = postViewModel.Category,
        };
    }

    public User ToUser(UserRequestDto userDTO)
    {
        return new User()
        {
            Username = userDTO.Username,
            Email = userDTO.Email,
            FirstName = userDTO.FirstName,
            LastName = userDTO.LastName,
            Password = userDTO.Password,
            IsBlocked = false,
        };
    }

    public UserResponseDto ToUserResponseDto(User user)
    {
        return new UserResponseDto()
        {
            Id = user.Id,
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            IsAdmin = user.IsAdmin,
            IsBlocked = user.IsBlocked,
            Posts = user.Posts?.Select(x => ToPostResponseDto(x)).ToList() ?? new List<PostResponseDto>(),
            Comments = user.Comments?.Select(x => ToCommentResponseDto(x)).ToList() ?? new List<CommentResponseDto>(),
            PhoneNumber = user.PhoneNumber,
        };
    }

    public Reaction ToReaction(ReactionRequestDto reactionDTO)
    {
        return new Reaction()
        {
            Id = reactionDTO.Id,
            CreatedDate = DateTime.UtcNow,
            ReactionType = reactionDTO.ReactionType,
            UserId = reactionDTO.UserId,
            ReactionTarget = new ReactionTarget
            {
                PostId = reactionDTO.PostId,
                CommentId = reactionDTO.CommentId
            }
        };
    }

    public ReactionResponseDto ToReactionResponseDto(Reaction reaction)
    {
        return new ReactionResponseDto()
        {
            Id = reaction.Id,
            CreatedDate = reaction.CreatedDate,
            ReactionType = reaction.ReactionType,
            PostId = reaction.ReactionTarget?.PostId,
            CommentId = reaction.ReactionTarget?.CommentId,
            UserId = reaction.UserId,
        };
    }
}
