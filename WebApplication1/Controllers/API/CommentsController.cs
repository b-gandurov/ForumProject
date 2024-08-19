using Microsoft.AspNetCore.Mvc;
using ForumProject.Helpers;
using ForumProject.Models.DTOs;
using ForumProject.Services.Contracts;
using ForumProject.Models;

namespace ForumProject.Controllers.API
{
    /// <summary>
    /// Controller for managing comment-related API endpoints.
    /// </summary>
    [ApiController]
    [Route("api/posts/{postId}/comments")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IModelMapper _modelMapper;
        private readonly IAuthService _authService;

        public CommentController(ICommentService commentService, IModelMapper modelMapper, IAuthService authService)
        {
            _commentService = commentService;
            _modelMapper = modelMapper;
            _authService = authService;
        }

        /// <summary>
        /// Retrieves a comment by its ID.
        /// </summary>
        /// <param name="commentId">The ID of the comment.</param>
        /// <returns>An IActionResult containing the comment.</returns>
        [HttpGet("{commentId}")]
        public IActionResult GetCommentById(int commentId)
        {
            var comment = _commentService.GetCommentById(commentId);
            var commentDto = _modelMapper.ToCommentResponseDto(comment);
            return Ok(commentDto);
        }

        /// <summary>
        /// Retrieves comments by post ID.
        /// </summary>
        /// <param name="postId">The ID of the post.</param>
        /// <returns>An IActionResult containing the comments.</returns>
        [HttpGet]
        public IActionResult GetCommentsByPostId(int postId)
        {
            var comments = _commentService.GetCommentsByPostId(postId);
            var commentDtos = comments.Select(c => _modelMapper.ToCommentResponseDto(c));
            return Ok(commentDtos);
        }

        /// <summary>
        /// Retrieves replies by comment ID.
        /// </summary>
        /// <param name="commentId">The ID of the comment.</param>
        /// <returns>An IActionResult containing the replies.</returns>
        [HttpGet("{commentId}/replies")]
        public IActionResult GetRepliesByCommentId(int commentId)
        {
            var replies = _commentService.GetRepliesByCommentId(commentId);
            var replyDtos = replies.Select(r => _modelMapper.ToCommentResponseDto(r));
            return Ok(replyDtos);
        }

        /// <summary>
        /// Adds a new comment to a post.
        /// </summary>
        /// <param name="credentials">The credentials of the User.</param>
        /// <param name="postId">The ID of the post.</param>
        /// <param name="commentDto">The comment to add.</param>
        /// <returns>An IActionResult indicating success.</returns>
        [HttpPost]
        [RequireAuthorization]
        public ActionResult AddComment(int postId, [FromBody] CommentRequestDto commentDto)
        {
            User user = (User)HttpContext.Items["User"];
            commentDto.UserId = user.Id;
            commentDto.PostId = postId;
            var comment = _modelMapper.ToComment(commentDto);
            _commentService.AddComment(comment);
            return CreatedAtAction(nameof(GetCommentById), new { postId, commentId = comment.Id }, _modelMapper.ToCommentResponseDto(comment));

        }


        /// <summary>
        /// Adds a new reply to a comment.
        /// </summary>
        /// <param name="credentials">The credentials of the User.</param>
        /// <param name="postId">The ID of the post.</param>
        /// <param name="parrentCommentId">The ID of the parent comment.</param>
        /// <param name="commentDto">The reply to add.</param>
        /// <returns>An IActionResult indicating success.</returns>
        [HttpPost("{parrentCommentId}/replies")]
        [RequireAuthorization]
        public ActionResult AddReply(int postId, int parrentCommentId, [FromBody] CommentRequestDto commentDto)
        {
            User user = (User)HttpContext.Items["User"];
            commentDto.UserId = user.Id;
            commentDto.PostId = postId;
            commentDto.ParentCommentId = parrentCommentId;
            var reply = _modelMapper.ToComment(commentDto);
            _commentService.AddReply(reply);
            return CreatedAtAction(nameof(GetCommentById), new { postId, parrentCommentId = reply.Id }, _modelMapper.ToCommentResponseDto(reply));
        }

        /// <summary>
        /// Updates an existing comment.
        /// </summary>
        /// <param name="postId">The ID of the post.</param>
        /// <param name="commentId">The ID of the comment to update.</param>
        /// <param name="credentials">The credentials of the User.</param>
        /// <param name="commentDto">The updated comment details.</param>
        /// <returns>An IActionResult containing the updated comment.</returns>
        [HttpPut("{commentId}")]
        [RequireAuthorization]
        public ActionResult UpdateComment(int postId, int commentId, [FromBody] CommentRequestDto commentDto)
        {
            User user = (User)HttpContext.Items["User"];
            var comment = _commentService.GetCommentById(commentId);
            commentDto.UserId = user.Id;
            commentDto.PostId = postId;
            comment.Content = commentDto.Content;
            _commentService.UpdateComment(comment);
            return Ok(_modelMapper.ToCommentResponseDto(comment));
        }

        /// <summary>
        /// Deletes a comment by its ID.
        /// </summary>
        /// <param name="commentId">The ID of the comment to delete.</param>
        /// <returns>An IActionResult indicating success.</returns>
        [HttpDelete("{commentId}")]
        [RequireAuthorization]
        public ActionResult DeleteComment(int commentId)
        {
            _commentService.DeleteComment(commentId);
            return NoContent();
        }
    }
}
