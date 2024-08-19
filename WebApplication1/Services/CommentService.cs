using ForumProject.Models;
using ForumProject.Repositories.Contracts;
using ForumProject.Services.Contracts;

namespace ForumProject.Services
{
    /// <summary>
    /// Service for managing comment-related business logic.
    /// </summary>
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        /// <summary>
        /// Retrieves a comment by its ID.
        /// </summary>
        /// <param name="id">The ID of the comment.</param>
        /// <returns>The comment with the specified ID.</returns>
        public Comment GetCommentById(int id)
        {
            return _commentRepository.GetCommentById(id);
        }

        /// <summary>
        /// Retrieves comments by post ID.
        /// </summary>
        /// <param name="postId">The ID of the post.</param>
        /// <returns>An IEnumerable of comments for the specified post.</returns>
        public IEnumerable<Comment> GetCommentsByPostId(int postId)
        {
            return _commentRepository.GetCommentsByPostId(postId);
        }

        /// <summary>
        /// Retrieves replies by comment ID.
        /// </summary>
        /// <param name="commentId">The ID of the comment.</param>
        /// <returns>An IEnumerable of replies for the specified comment.</returns>
        public IEnumerable<Comment> GetRepliesByCommentId(int commentId)
        {
            return _commentRepository.GetRepliesByCommentId(commentId);
        }

        /// <summary>
        /// Adds a new comment.
        /// </summary>
        /// <param name="comment">The comment to add.</param>
        public void AddComment(Comment comment)
        {
            _commentRepository.AddComment(comment);
        }

        /// <summary>
        /// Adds a new reply to a comment.
        /// </summary>
        /// <param name="reply">The reply to add.</param>
        public void AddReply(Comment reply)
        {
            _commentRepository.AddComment(reply);
        }

        /// <summary>
        /// Updates an existing comment.
        /// </summary>
        /// <param name="comment">The comment to update.</param>
        public void UpdateComment(Comment comment)
        {
            _commentRepository.UpdateComment(comment);
        }

        /// <summary>
        /// Deletes a comment by its ID.
        /// </summary>
        /// <param name="id">The ID of the comment to delete.</param>
        public void DeleteComment(int id)
        {
            _commentRepository.DeleteComment(id);
        }
    }
}
