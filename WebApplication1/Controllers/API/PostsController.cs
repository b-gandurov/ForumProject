using Microsoft.AspNetCore.Mvc;
using ForumProject.Helpers;
using ForumProject.Models.DTOs;
using ForumProject.Models.QueryParameters;
using ForumProject.Services.Contracts;
using ForumProject.Models;

namespace ForumProject.Controllers.API
{
    /// <summary>
    /// Controller for managing post-related API endpoints.
    /// </summary>
    [ApiController]
    [Route("api/posts")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IModelMapper _modelMapper;
        private readonly IAuthService _authService;

        public PostsController(IPostService postService, IModelMapper modelMapper, IAuthService authService)
        {
            _postService = postService;
            _modelMapper = modelMapper;
            _authService = authService;
        }

        /// <summary>
        /// Retrieves all posts.
        /// </summary>
        /// <returns>An IActionResult containing all posts.</returns>
        [HttpGet("")]
        public IActionResult GetAllPosts()
        {
            var posts = _postService.GetAllPosts();
            var postDtos = posts.Select(p => _modelMapper.ToPostResponseDto(p));
            return Ok(postDtos);
        }

        /// <summary>
        /// Retrieves a post by its ID.
        /// </summary>
        /// <param name="postId">The ID of the post.</param>
        /// <returns>An IActionResult containing the post.</returns>
        [HttpGet("{postId}")]
        public IActionResult GetPostById(int postId)
        {
            var post = _postService.GetPostById(postId);
            if (post == null)
            {
                return NotFound($"Post with ID {postId} not found.");
            }
            var postDto = _modelMapper.ToPostResponseDto(post);
            return Ok(postDto);
        }

        /// <summary>
        /// Adds a new post.
        /// </summary>
        /// <param name="credentials">The credentials of the User.</param>
        /// <param name="postDto">The post to add.</param>
        /// <returns>An IActionResult indicating success.</returns>
        [HttpPost("")]
        [RequireAuthorization]
        public ActionResult AddPost([FromBody] PostRequestDto postDto)
        {
            var user = (User)HttpContext.Items["User"];
            var post = _modelMapper.ToPost(postDto);
            post.User = user;
            post.UserId = user.Id;
            _postService.AddPost(post);

            return CreatedAtAction(nameof(GetPostById), new { postId = post.Id }, _modelMapper.ToPostResponseDto(post));
        }


        /// <summary>
        /// Updates an existing post.
        /// </summary>
        /// <param name="postId">The ID of the post to update.</param>
        /// <param name="postDto">The updated post details.</param>
        /// <returns>An IActionResult containing the updated post.</returns>
        [HttpPut("{postId}")]
        [RequireAuthorization]
        public ActionResult UpdatePost(int postId, [FromBody] PostRequestDto postDto)
        {
            var post = _postService.GetPostById(postId);
            if (post == null)
            {
                return NotFound($"Post with ID {postId} not found.");
            }
            post.Title = postDto.Title ?? post.Title;
            post.Content = postDto.Content ?? post.Content;
            _postService.UpdatePost(post);
            return Ok(_modelMapper.ToPostResponseDto(post));
        }

        /// <summary>
        /// Deletes a post by its ID.
        /// </summary>
        /// <param name="postId">The ID of the post to delete.</param>
        /// <returns>An IActionResult indicating success.</returns>
        [HttpDelete("{postId}")]
        [RequireAuthorization]
        public ActionResult DeletePost(int postId)
        {
            var post = _postService.GetPostById(postId);
            if (post == null)
            {
                return NotFound($"Post with ID {postId} not found.");
            }
            _postService.DeletePost(postId);
            return Ok("Post deleted successfully");
        }

        /// <summary>
        /// Retrieves the top commented posts.
        /// </summary>
        /// <param name="count">The number of top commented posts to retrieve.</param>
        /// <returns>An IActionResult containing the top commented posts.</returns>
        [HttpGet("top")]
        public IActionResult GetTopPosts([FromQuery] int count = 10)
        {
            var posts = _postService.GetTopCommentedPosts(count);
            var postDtos = posts.Select(p => _modelMapper.ToPostResponseDto(p));
            return Ok(postDtos);
        }

        /// <summary>
        /// Retrieves the most recent posts.
        /// </summary>
        /// <param name="count">The number of recent posts to retrieve.</param>
        /// <returns>An IActionResult containing the recent posts.</returns>
        [HttpGet("recent")]
        public IActionResult GetRecentPosts([FromQuery] int count = 10)
        {
            var posts = _postService.GetRecentPosts(count);
            var postDtos = posts.Select(p => _modelMapper.ToPostResponseDto(p));
            return Ok(postDtos);
        }

        /// <summary>
        /// Retrieves posts by a specific User ID.
        /// </summary>
        /// <param name="userId">The ID of the User.</param>
        /// <returns>An IActionResult containing the posts by the specified User.</returns>
        [HttpGet("User/{userId}")]
        public IActionResult GetUserPosts(int userId)
        {
            var posts = _postService.GetPostsByUserId(userId);
            var postDtos = posts.Select(p => _modelMapper.ToPostResponseDto(p));
            return Ok(postDtos);
        }

        /// <summary>
        /// Filters posts based on query parameters.
        /// </summary>
        /// <param name="filterParameters">The filter parameters.</param>
        /// <returns>An IActionResult containing the filtered posts.</returns>
        [HttpGet("filter")]
        public IActionResult FilterPosts([FromQuery] PostQueryParameters filterParameters)
        {
            var posts = _postService.FilterBy(filterParameters);
            var postDtos = posts.Select(p => _modelMapper.ToPostResponseDto(p));
            return Ok(postDtos);
        }
    }
}
