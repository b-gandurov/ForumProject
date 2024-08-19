using ForumProject.Models;
using ForumProject.Models.Enums;
using ForumProject.Models.QueryParameters;
using ForumProject.Repositories.Contracts;
using ForumProject.Services.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace ForumProject.Services
{
    /// <summary>
    /// Service for managing post-related business logic.
    /// </summary>
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        /// <summary>
        /// Retrieves a post by its ID.
        /// </summary>
        /// <param name="id">The ID of the post.</param>
        /// <returns>The post with the specified ID.</returns>
        public Post GetPostById(int id)
        {
            var post = _postRepository.GetPostById(id);
            return post;
        }

        /// <summary>
        /// Retrieves all posts.
        /// </summary>
        /// <returns>An IEnumerable of all posts.</returns>
        public IEnumerable<Post> GetAllPosts()
        {
            var posts = _postRepository.GetAllPosts().ToList();
            return posts;
        }

        /// <summary>
        /// Retrieves the top commented posts.
        /// </summary>
        /// <param name="count">The number of top commented posts to retrieve.</param>
        /// <returns>An IEnumerable of the top commented posts.</returns>
        public IEnumerable<Post> GetTopCommentedPosts(int count)
        {
            var posts = _postRepository.GetTopCommentedPosts(count).ToList();
            return posts;
        }

        public IEnumerable<Post> GetTopReactedPosts(int count)
        {
            var posts = _postRepository.GetTopReactedPosts(count).ToList();
            return posts;
        }

        /// <summary>
        /// Retrieves the most recent posts.
        /// </summary>
        /// <param name="count">The number of recent posts to retrieve.</param>
        /// <returns>An IEnumerable of the recent posts.</returns>
        public IEnumerable<Post> GetRecentPosts(int count)
        {
            var posts = _postRepository.GetRecentPosts(count).ToList();
            return posts;
        }

        /// <summary>
        /// Adds a new post.
        /// </summary>
        /// <param name="post">The post to add.</param>
        public void AddPost(Post post)
        {
            _postRepository.AddPost(post);
        }

        /// <summary>
        /// Updates an existing post.
        /// </summary>
        /// <param name="post">The post to update.</param>
        public void UpdatePost(Post post)
        {
            _postRepository.UpdatePost(post);
        }

        /// <summary>
        /// Deletes a post by its ID.
        /// </summary>
        /// <param name="id">The ID of the post to delete.</param>
        public void DeletePost(int id)
        {
            _postRepository.DeletePost(id);
        }

        /// <summary>
        /// Retrieves posts by a specific User ID.
        /// </summary>
        /// <param name="userId">The ID of the User.</param>
        /// <returns>An IEnumerable of posts by the specified User.</returns>
        public IEnumerable<Post> GetPostsByUserId(int userId)
        {
            var posts = _postRepository.GetPostsByUserId(userId).ToList();
            return posts;
        }

        /// <summary>
        /// Filters posts based on query parameters.
        /// </summary>
        /// <param name="filterParameters">The filter parameters.</param>
        /// <returns>An IEnumerable of filtered posts.</returns>
        public IEnumerable<Post> FilterBy(PostQueryParameters filterParameters)
        {
            var posts = _postRepository.FilterBy(filterParameters).ToList();
            return posts;
        }

        public int GetTotalCount(PostQueryParameters filterParameters)
        {
            return _postRepository.GetTotalCount(filterParameters);
        }
    }
}
