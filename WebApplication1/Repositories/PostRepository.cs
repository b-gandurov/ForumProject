using ForumProject.Data;
using ForumProject.Models.QueryParameters;
using ForumProject.Models;
using Microsoft.EntityFrameworkCore;
using ForumProject.Exceptions;
using ForumProject.Repositories.Contracts;
using ForumProject.Resources;
using ForumProject.Models.Enums;
using System.Linq.Expressions;

namespace ForumProject.Repositories
{
    /// <summary>
    /// Repository for managing post-related data operations.
    /// </summary>
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns an IQueryable with eager loading for related entities.
        /// </summary>
        private IQueryable<Post> GetPostQueryWithEagerLoading()
        {
            return _context.Posts
                    .Include(p => p.User)
                    .Include(p => p.Comments)
                        .ThenInclude(c => c.User)
                    .Include(p => p.Comments)
                        .ThenInclude(c => c.ReactionTargets)
                            .ThenInclude(rt => rt.Reactions)
                            .ThenInclude(r => r.User)
                    .Include(p => p.ReactionTargets)
                        .ThenInclude(rt => rt.Reactions).ThenInclude(r => r.User);
        }

        /// <summary>
        /// Retrieves a post by its ID.
        /// </summary>
        /// <param name="id">The ID of the post.</param>
        /// <returns>The post with the specified ID.</returns>
        /// <exception cref="EntityNotFoundException">Thrown if the post is not found.</exception>
        public Post GetPostById(int id)
        {
            var post = GetPostQueryWithEagerLoading()
                .FirstOrDefault(p => p.Id == id);

            if (post == null)
            {
                throw new EntityNotFoundException($"Post with ID {id} not found.");
            }

            return post;
        }

        /// <summary>
        /// Retrieves all posts.
        /// </summary>
        /// <returns>An IQueryable of all posts.</returns>
        public IQueryable<Post> GetAllPosts()
        {
            var posts = GetPostQueryWithEagerLoading();

            return posts;
        }

        /// <summary>
        /// Retrieves the top commented posts.
        /// </summary>
        /// <param name="count">The number of top commented posts to retrieve.</param>
        /// <returns>An IQueryable of the top commented posts.</returns>
        public IQueryable<Post> GetTopCommentedPosts(int count)
        {
            return GetPostQueryWithEagerLoading()
                .OrderByDescending(p => p.Comments.Count)
                .Take(count);
        }

        public IQueryable<Post> GetTopReactedPosts(int count)
        {
            return GetPostQueryWithEagerLoading()
                .OrderByDescending(p => p.ReactionTargets.Count)
                .Take(count);
        }



        /// <summary>
        /// Retrieves the most recent posts.
        /// </summary>
        /// <param name="count">The number of recent posts to retrieve.</param>
        /// <returns>An IQueryable of the recent posts.</returns>
        public IQueryable<Post> GetRecentPosts(int count)
        {
            return GetPostQueryWithEagerLoading()
                .OrderByDescending(p => p.CreatedAt)
                .Take(count);
        }

        /// <summary>
        /// Adds a new post.
        /// </summary>
        /// <param name="post">The post to add.</param>
        public void AddPost(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }

        /// <summary>
        /// Updates an existing post.
        /// </summary>
        /// <param name="post">The post to update.</param>
        /// <exception cref="EntityNotFoundException">Thrown if the post is not found.</exception>
        public void UpdatePost(Post post)
        {
            var existingPost = _context.Posts.Find(post.Id);
            if (existingPost == null)
            {
                throw new EntityNotFoundException(string.Format(ErrorMessages.PostNotFound, post.Id));
            }

            _context.Entry(existingPost).CurrentValues.SetValues(post);
            _context.SaveChanges();
        }

        /// <summary>
        /// Deletes a post by its ID.
        /// </summary>
        /// <param name="id">The ID of the post to delete.</param>
        /// <exception cref="EntityNotFoundException">Thrown if the post is not found.</exception>
        public void DeletePost(int id)
        {
            var post = _context.Posts.Find(id);
            if (post == null)
            {
                throw new EntityNotFoundException(string.Format(ErrorMessages.PostNotFound, id));
            }

            _context.Posts.Remove(post);
            _context.SaveChanges();
        }

        /// <summary>
        /// Retrieves posts by a specific User ID.
        /// </summary>
        /// <param name="userId">The ID of the User.</param>
        /// <returns>An IQueryable of posts by the specified User.</returns>
        /// <exception cref="EntityNotFoundException">Thrown if the User is not found.</exception>
        public IQueryable<Post> GetPostsByUserId(int userId)
        {
            if (!_context.Users.Any(u => u.Id == userId))
            {
                throw new EntityNotFoundException(string.Format(ErrorMessages.UserNotFound, userId));
            }

            return GetPostQueryWithEagerLoading()
                .Where(p => p.UserId == userId);
        }

        /// <summary>
        /// Filters posts based on query parameters.
        /// </summary>
        /// <param name="filterParameters">The filter parameters.</param>
        /// <returns>An IQueryable of filtered posts.</returns>
        public IQueryable<Post> FilterBy(PostQueryParameters filterParameters)
        {
            var posts = GetPostQueryWithEagerLoading();

            if (!string.IsNullOrWhiteSpace(filterParameters.Title))
            {
                posts = posts.Where(p => p.Title.Contains(filterParameters.Title));
            }
            if (filterParameters.CreatedAfter.HasValue)
            {
                posts = posts.Where(p => p.CreatedAt >= filterParameters.CreatedAfter.Value);
            }
            if (filterParameters.CreatedBefore.HasValue)
            {
                posts = posts.Where(p => p.CreatedAt <= filterParameters.CreatedBefore.Value);
            }
            if (!string.IsNullOrEmpty(filterParameters.UserName))
            {
                posts = posts.Where(p => p.User.Username == filterParameters.UserName);
            }
            if (filterParameters.Category.HasValue)
            {
                posts = posts.Where(p => p.Category == filterParameters.Category.Value);
            }

            var sortPropertyMapping = new Dictionary<string, Expression<Func<Post, object>>>()
                    {
                        { "CreatedAt", p => p.CreatedAt },
                        { "Title", p => p.Title }
                     
                    };

            if (!string.IsNullOrEmpty(filterParameters.SortBy) && sortPropertyMapping.ContainsKey(filterParameters.SortBy))
            {
                if (filterParameters.SortOrder.ToLower() == "asc")
                {
                    posts = posts.OrderBy(sortPropertyMapping[filterParameters.SortBy]);
                }
                else
                {
                    posts = posts.OrderByDescending(sortPropertyMapping[filterParameters.SortBy]);
                }
            }

            var skip = (filterParameters.PageNumber - 1) * filterParameters.PageSize;
            return posts.Skip(skip).Take(filterParameters.PageSize);
        }


        public int GetTotalCount(PostQueryParameters filterParameters)
        {
            var posts = _context.Posts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterParameters.Title))
            {
                posts = posts.Where(p => p.Title.Contains(filterParameters.Title));
            }
            if (filterParameters.CreatedAfter.HasValue)
            {
                posts = posts.Where(p => p.CreatedAt >= filterParameters.CreatedAfter.Value);
            }
            if (filterParameters.CreatedBefore.HasValue)
            {
                posts = posts.Where(p => p.CreatedAt <= filterParameters.CreatedBefore.Value);
            }
            if (!string.IsNullOrEmpty(filterParameters.UserName))
            {
                posts = posts.Where(p => p.User.Username == filterParameters.UserName);
            }
            if (filterParameters.Category.HasValue)
            {
                posts = posts.Where(p => p.Category == filterParameters.Category.Value);
            }

            return posts.Count();
        }

    }
}
