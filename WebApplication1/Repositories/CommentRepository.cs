using ForumProject.Data;
using ForumProject.Models.QueryParameters;
using ForumProject.Models;
using Microsoft.EntityFrameworkCore;
using ForumProject.Repositories.Contracts;
using ForumProject.Exceptions;
using ForumProject.Resources;
using ForumProject.Models.ViewModels;

namespace ForumProject.Repositories
{
    /// <summary>
    /// Repository for managing comment-related data operations.
    /// </summary>
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns an IQueryable with eager loading for related entities.
        /// </summary>
        private IQueryable<Comment> GetCommentWithDetails()
        {
            return _context.Comments
                .Include(c => c.User)
                .Include(c => c.ReactionTargets)
                    .ThenInclude(rt => rt.Reactions)
                    .ThenInclude(r => r.User)
                .Include(c => c.Replies)
                    .ThenInclude(r => r.User);
        }

        /// <summary>
        /// Retrieves a comment by its ID.
        /// </summary>
        /// <param name="id">The ID of the comment.</param>
        /// <returns>The comment with the specified ID.</returns>
        /// <exception cref="EntityNotFoundException">Thrown if the comment is not found.</exception>
        public Comment GetCommentById(int id)
        {;



            var comment = GetCommentWithDetails()
                .FirstOrDefault(c => c.Id == id);

            if (comment == null)
            {
                throw new EntityNotFoundException(string.Format(ErrorMessages.CommentNotFound, id));
            }

            var reactions = comment.ReactionTargets.SelectMany(rt => rt.Reactions)
                          .GroupBy(r => r.ReactionType)
                          .Select(g => new ReactionViewModel
                          {
                              ReactionType = g.Key.ToString(),
                              Count = g.Count()
                          })
                          .ToList();

            return comment;
        }

        /// <summary>
        /// Retrieves comments by post ID.
        /// </summary>
        /// <param name="postId">The ID of the post.</param>
        /// <returns>An IQueryable of comments for the specified post.</returns>
        /// <exception cref="EntityNotFoundException">Thrown if the post is not found.</exception>
        public IQueryable<Comment> GetCommentsByPostId(int postId)
        {
            if (!_context.Posts.Any(p => p.Id == postId))
            {
                throw new EntityNotFoundException(string.Format(ErrorMessages.PostNotFound, postId));
            }

            return GetCommentWithDetails()
                .Where(c => c.Post.Id == postId);
        }

        /// <summary>
        /// Retrieves replies by comment ID.
        /// </summary>
        /// <param name="commentId">The ID of the comment.</param>
        /// <returns>An IQueryable of replies for the specified comment.</returns>
        /// <exception cref="EntityNotFoundException">Thrown if the comment is not found.</exception>
        public IQueryable<Comment> GetRepliesByCommentId(int commentId)
        {
            if (!_context.Comments.Any(c => c.Id == commentId))
            {
                throw new EntityNotFoundException(string.Format(ErrorMessages.CommentNotFound, commentId));
            }

            return GetCommentWithDetails()
                .Where(c => c.ParentCommentId == commentId);
        }

        /// <summary>
        /// Adds a new comment.
        /// </summary>
        /// <param name="comment">The comment to add.</param>
        public void AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }

        /// <summary>
        /// Updates an existing comment.
        /// </summary>
        /// <param name="comment">The comment to update.</param>
        /// <exception cref="EntityNotFoundException">Thrown if the comment is not found.</exception>
        public void UpdateComment(Comment comment)
        {
            var existingComment = _context.Comments.Find(comment.Id);
            if (existingComment == null)
            {
                throw new EntityNotFoundException(string.Format(ErrorMessages.CommentNotFound, comment.Id));
            }

            _context.Entry(existingComment).CurrentValues.SetValues(comment);
            _context.SaveChanges();
        }

        /// <summary>
        /// Deletes a comment by its ID.
        /// </summary>
        /// <param name="id">The ID of the comment to delete.</param>
        /// <exception cref="EntityNotFoundException">Thrown if the comment is not found.</exception>
        public void DeleteComment(int id)
        {
            var comment = _context.Comments.Find(id);
            if (comment == null)
            {
                throw new EntityNotFoundException(string.Format(ErrorMessages.CommentNotFound, id));
            }

            comment.DeletedAt = DateTime.UtcNow; // Soft delete by setting DeletedAt
            _context.Comments.Update(comment); // Update the comment instead of removing
            _context.SaveChanges();
        }


        /// <summary>
        /// Filters comments based on query parameters.
        /// </summary>
        /// <param name="filterParameters">The filter parameters.</param>
        /// <returns>An IQueryable of filtered comments.</returns>
        public IQueryable<Comment> FilterBy(CommentQueryParameters filterParameters)
        {
            var query = GetCommentWithDetails();

            if (filterParameters.PostId.HasValue)
            {
                query = query.Where(c => c.Post.Id == filterParameters.PostId);
            }

            if (filterParameters.UserId.HasValue)
            {
                query = query.Where(c => c.User.Id == filterParameters.UserId);
            }

            if (filterParameters.CreatedAfter.HasValue)
            {
                query = query.Where(c => c.CreatedDate >= filterParameters.CreatedAfter.Value);
            }

            if (filterParameters.CreatedBefore.HasValue)
            {
                query = query.Where(c => c.CreatedDate <= filterParameters.CreatedBefore.Value);
            }

            return query;
        }
    }
}
