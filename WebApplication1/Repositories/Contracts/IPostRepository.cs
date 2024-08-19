using ForumProject.Models;
using ForumProject.Models.QueryParameters;

namespace ForumProject.Repositories.Contracts
{
    public interface IPostRepository
    {
        Post GetPostById(int id);
        IQueryable<Post> GetAllPosts();
        IQueryable<Post> GetTopCommentedPosts(int count);
        IQueryable<Post> GetRecentPosts(int count);
        void AddPost(Post post);
        void UpdatePost(Post post);
        void DeletePost(int id);
        IQueryable<Post> GetPostsByUserId(int userId);

        public IQueryable<Post> GetTopReactedPosts(int count);
        IQueryable<Post> FilterBy(PostQueryParameters filterParameters);

        public int GetTotalCount(PostQueryParameters filterParameters);
    }

}
