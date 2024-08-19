using ForumProject.Models;
using ForumProject.Models.QueryParameters;

namespace ForumProject.Repositories.Contracts
{
    public interface ICommentRepository
    {
        Comment GetCommentById(int id);
        IQueryable<Comment> GetCommentsByPostId(int postId);
        IQueryable<Comment> GetRepliesByCommentId(int commentId);
        void AddComment(Comment comment);
        void UpdateComment(Comment comment);
        void DeleteComment(int id);
        IQueryable<Comment> FilterBy(CommentQueryParameters filterParameters);
    }

}
