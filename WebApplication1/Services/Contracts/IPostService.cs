using ForumProject.Models;
using ForumProject.Models.QueryParameters;

namespace ForumProject.Services.Contracts
{
    public interface IPostService
    {
        int GetTotalCount(PostQueryParameters filterParameters);
        public Post GetPostById(int id);

        public IEnumerable<Post> GetAllPosts();

        public IEnumerable<Post> GetTopCommentedPosts(int count);

        public IEnumerable<Post> GetRecentPosts(int count);

        public IEnumerable<Post> GetTopReactedPosts(int count);

        public void AddPost(Post post);


        public void UpdatePost(Post post);

        public void DeletePost(int id);

        public IEnumerable<Post> GetPostsByUserId(int userId);

        public IEnumerable<Post> FilterBy(PostQueryParameters filterParameters);
    }

}
