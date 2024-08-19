using ForumProject.Models.DTOs;
using ForumProject.Repositories.Contracts;
using ForumProject.Services.Contracts;

namespace ForumProject.Services
{
    /// <summary>
    /// Service for managing reactions to posts.
    /// </summary>
    public class ReactionService : IReactionService
    {
        private readonly IReactionRepository _reactionRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactionService"/> class.
        /// </summary>
        /// <param name="reactionRepository">Repository for reaction operations.</param>
        public ReactionService(IReactionRepository reactionRepository)
        {
            this._reactionRepository = reactionRepository;
        }

        /// <summary>
        /// Adds a new reaction.
        /// </summary>
        /// <param name="reactionDto">Reaction data transfer object containing reaction details.</param>
        public void AddReaction(ReactionRequestDto reactionDto)
        {
            _reactionRepository.AddReaction(reactionDto);
        }

        /// <summary>
        /// Deletes a reaction by its ID.
        /// </summary>
        /// <param name="id">The ID of the reaction to delete.</param>
        /// <exception cref="ArgumentException">Thrown when the reaction is not found.</exception>
        public void DeleteReaction(int id)
        {

            _reactionRepository.DeleteReaction(id);
        }

        /// <summary>
        /// Gets a reaction by its ID.
        /// </summary>
        /// <param name="id">The ID of the reaction to retrieve.</param>
        /// <returns>Reaction response data transfer object.</returns>
        /// <exception cref="ArgumentException">Thrown when the ID is invalid.</exception>
        public ReactionResponseDto GetReactionById(int id)
        {

            return _reactionRepository.GetReactionById(id);
        }

        /// <summary>
        /// Gets all reactions for a specific post.
        /// </summary>
        /// <param name="postId">The ID of the post.</param>
        /// <returns>List of reactions for the post.</returns>
        /// <exception cref="ArgumentException">Thrown when the post ID is invalid.</exception>
        public IEnumerable<ReactionResponseDto> GetReactionsByPostId(int postId)
        {
            return _reactionRepository.GetReactionsByPostId(postId);
        }

        /// <summary>
        /// Gets all reactions for a specific comment.
        /// </summary>
        /// <param name="commentId">The ID of the comment.</param>
        /// <returns>List of reactions for the comment.</returns>
        /// <exception cref="ArgumentException">Thrown when the comment ID is invalid.</exception>
        public IEnumerable<ReactionResponseDto> GetReactionsByCommentId(int commentId)
        {

            return _reactionRepository.GetReactionsByCommentId(commentId);
        }

        /// <summary>
        /// Updates an existing reaction.
        /// </summary>
        /// <param name="reactionDto">Reaction data transfer object containing updated reaction details.</param>
        /// <exception cref="ArgumentException">Thrown when the reaction DTO is null.</exception>
        public void UpdateReaction(ReactionRequestDto reactionDto)
        {
            _reactionRepository.UpdateReaction(reactionDto);
        }
    }
}
