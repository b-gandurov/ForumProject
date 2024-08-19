using ForumProject.Models;

namespace ForumProject.Services.Contracts
{
    public interface ICommentService
    {
        public Comment GetCommentById(int id);

        public IEnumerable<Comment> GetCommentsByPostId(int postId);

        public IEnumerable<Comment> GetRepliesByCommentId(int commentId);

        public void AddComment(Comment comment);

        public void AddReply(Comment reply);

        public void UpdateComment(Comment comment);

        public void DeleteComment(int id);
    }

}
