using ForumProject.Data;
using ForumProject.Exceptions;
using ForumProject.Helpers;
using ForumProject.Models;
using ForumProject.Models.DTOs;
using ForumProject.Models.QueryParameters;
using ForumProject.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ForumProject.Repositories
{
    /// <summary>
    /// Repository for managing reactions to posts and comments.
    /// </summary>
    public class ReactionRepository : IReactionRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IModelMapper _modelMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactionRepository"/> class.
        /// </summary>
        /// <param name="dbContext">Database context for accessing reactions data.</param>
        /// <param name="modelMapper">Model mapper for converting entities and DTOs.</param>
        public ReactionRepository(ApplicationDbContext dbContext, IModelMapper modelMapper)
        {
            _dbContext = dbContext;
            _modelMapper = modelMapper;
        }

        /// <summary>
        /// Returns an IQueryable with eager loading for related entities.
        /// </summary>
        private IQueryable<Reaction> GetReactionQueryWithEagerLoading()
        {
            return _dbContext.Reactions
                .Include(r => r.User)
                //.ThenInclude(u=>u.Username)
                .Include(r => r.ReactionTarget)
                    .ThenInclude(rt => rt.Post)
                .Include(r => r.ReactionTarget)
                    .ThenInclude(rt => rt.Comment);
        }

        /// <summary>
        /// Adds a new reaction or updates an existing one.
        /// </summary>
        /// <param name="reactionDto">Reaction data transfer object containing reaction details.</param>
        public void AddReaction(ReactionRequestDto reactionDto)
        {
            var existingReaction = GetReactionQueryWithEagerLoading()
                .FirstOrDefault(r => r.UserId == reactionDto.UserId
                                     && r.ReactionTarget.PostId == reactionDto.PostId
                                     && r.ReactionTarget.CommentId == reactionDto.CommentId);

            if (existingReaction != null)
            {
                reactionDto.Id = existingReaction.Id;
                UpdateReaction(reactionDto);
            }
            else
            {
                var reactionToAdd = _modelMapper.ToReaction(reactionDto);
                _dbContext.Reactions.Add(reactionToAdd);
                _dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes a reaction by marking it as deleted.
        /// </summary>
        /// <param name="id">The ID of the reaction to delete.</param>
        public void DeleteReaction(int id)
        {
            var reaction = GetReactionEntity(id);

            reaction.DeletedAt = DateTime.Now;

            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Filters reactions based on specified query parameters.
        /// </summary>
        /// <param name="filterParameters">The query parameters for filtering reactions.</param>
        /// <returns>List of reactions that match the filter criteria.</returns>
        public IEnumerable<ReactionResponseDto> FilterBy(ReactionQueryParameters filterParameters)
        {
            IQueryable<Reaction> query = GetReactionQueryWithEagerLoading()
                .Where(r =>
                    (r.ReactionTarget.PostId == filterParameters.PostId || r.ReactionTarget.CommentId == filterParameters.CommentId)
                    && r.UserId == filterParameters.UserId
                    && r.ReactionType == filterParameters.Type);

            return query.Select(x => _modelMapper.ToReactionResponseDto(x));
        }

        /// <summary>
        /// Gets a reaction by its ID.
        /// </summary>
        /// <param name="id">The ID of the reaction to retrieve.</param>
        /// <returns>Reaction data transfer object.</returns>
        public ReactionResponseDto GetReactionById(int id)
        {
            var reaction = GetReactionQueryWithEagerLoading()
                .FirstOrDefault(r => r.Id == id);

            if (reaction == null)
            {
                throw new Exception("Reaction not found");
            }

            return _modelMapper.ToReactionResponseDto(reaction);
        }

        /// <summary>
        /// Gets all reactions for a specific comment.
        /// </summary>
        /// <param name="commentId">The ID of the comment.</param>
        /// <returns>List of reactions for the comment.</returns>
        public IEnumerable<ReactionResponseDto> GetReactionsByCommentId(int commentId)
        {
            var reactions = GetReactionQueryWithEagerLoading()
                .Where(r => r.ReactionTarget.CommentId == commentId)
                .Select(r => _modelMapper.ToReactionResponseDto(r));

            return reactions;
        }

        /// <summary>
        /// Retrieves the reaction entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the reaction entity to retrieve.</param>
        /// <returns>The reaction entity.</returns>
        /// <exception cref="Exception">Thrown when the reaction is not found.</exception>
        private Reaction GetReactionEntity(int id)
        {
            var reaction = GetReactionQueryWithEagerLoading()
                .FirstOrDefault(r => r.Id == id);

            if (reaction == null)
            {
                throw new Exception("Reaction not found");
            }

            return reaction;
        }

        /// <summary>
        /// Gets all reactions for a specific post.
        /// </summary>
        /// <param name="postId">The ID of the post.</param>
        /// <returns>List of reactions for the post.</returns>
        public IEnumerable<ReactionResponseDto> GetReactionsByPostId(int postId)
        {
            var reactions = GetReactionQueryWithEagerLoading()
                .Where(r => r.ReactionTarget.PostId == postId)
                .Select(r => _modelMapper.ToReactionResponseDto(r));

            return reactions;
        }

        /// <summary>
        /// Updates an existing reaction.
        /// </summary>
        /// <param name="reactionDto">Reaction data transfer object containing updated reaction details.</param>
        public void UpdateReaction(ReactionRequestDto reactionDto)
        {
            var reactionToUpdate = GetReactionEntity(reactionDto.Id);

            if (reactionToUpdate == null)
            {
                throw new Exception("Reaction not found");
            }

            reactionToUpdate.ReactionType = reactionDto.ReactionType;
            reactionToUpdate.CreatedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }
    }
}
