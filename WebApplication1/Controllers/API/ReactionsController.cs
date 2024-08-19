using Microsoft.AspNetCore.Mvc;
using ForumProject.Models.DTOs;
using ForumProject.Services.Contracts;

namespace ForumProject.Controllers.API
{
    /// <summary>
    /// Controller for managing reactions to posts and comments.
    /// </summary>
    [ApiController]
    [Route("api/reactions")]
    public class ReactionsController : ControllerBase
    {
        private readonly IReactionService _reactionService;
        private readonly IAuthService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactionsController"/> class.
        /// </summary>
        /// <param name="reactionService">Service for managing reactions.</param>
        /// <param name="authService">Service for authentication operations.</param>
        public ReactionsController(IReactionService reactionService, IAuthService authService)
        {
            _reactionService = reactionService;
            _authService = authService;
        }

        /// <summary>
        /// Gets a reaction by its ID.
        /// </summary>
        /// <param name="reactionId">The ID of the reaction.</param>
        /// <returns>The reaction if found; otherwise, a 404 status code.</returns>
        [HttpGet("{reactionId}")]
        public IActionResult GetReactionById(int reactionId)
        {
            var reaction = _reactionService.GetReactionById(reactionId);
            if (reaction == null)
            {
                return NotFound();
            }
            return Ok(reaction);
        }

        /// <summary>
        /// Gets all reactions for a specific post or comment.
        /// </summary>
        /// <param name="postId">The ID of the post (optional).</param>
        /// <param name="commentId">The ID of the comment (optional).</param>
        /// <returns>A list of reactions for the post or comment.</returns>
        [HttpGet]
        public IActionResult GetReactions([FromQuery] int? postId, [FromQuery] int? commentId)
        {
            if (postId.HasValue)
            {
                var reactions = _reactionService.GetReactionsByPostId(postId.Value);
                return Ok(reactions);
            }
            else if (commentId.HasValue)
            {
                var reactions = _reactionService.GetReactionsByCommentId(commentId.Value);
                return Ok(reactions);
            }
            else
            {
                return BadRequest("Either postId or commentId must be provided.");
            }
        }

        /// <summary>
        /// Adds a new reaction to a post or comment.
        /// </summary>
        /// <param name="credentials">The credentials of the User adding the reaction.</param>
        /// <param name="reactionDto">The data transfer object containing reaction details.</param>
        /// <returns>Confirmation message of successful addition.</returns>
        [HttpPost]
        [RequireAuthorization]
        public IActionResult AddReaction([FromHeader] string credentials, [FromBody] ReactionRequestDto reactionDto)
        {
            var user = _authService.Authenticate(credentials);
            reactionDto.UserId = user.Id;

            if (!reactionDto.PostId.HasValue && !reactionDto.CommentId.HasValue)
            {
                return BadRequest("Either PostId or CommentId must be provided.");
            }

            _reactionService.AddReaction(reactionDto);
            return Ok("Reaction added successfully");
        }

        /// <summary>
        /// Updates an existing reaction.
        /// </summary>
        /// <param name="reactionId">The ID of the reaction to update.</param>
        /// <param name="reactionDto">The data transfer object containing updated reaction details.</param>
        /// <returns>Confirmation message of successful update.</returns>
        [HttpPut("{reactionId}")]
        [RequireAuthorization]
        public IActionResult UpdateReaction(int reactionId, [FromBody] ReactionRequestDto reactionDto)
        {
            reactionDto.Id = reactionId;

            if (!reactionDto.PostId.HasValue && !reactionDto.CommentId.HasValue)
            {
                return BadRequest("Either PostId or CommentId must be provided.");
            }

            _reactionService.UpdateReaction(reactionDto);
            return Ok("Reaction updated successfully");
        }

        /// <summary>
        /// Deletes a reaction.
        /// </summary>
        /// <param name="reactionId">The ID of the reaction to delete.</param>
        /// <returns>Confirmation message of successful deletion.</returns>
        [HttpDelete("{reactionId}")]
        [RequireAuthorization]
        public IActionResult DeleteReaction(int reactionId)
        {
            var reaction = _reactionService.GetReactionById(reactionId);
            if (reaction == null)
            {
                return NotFound();
            }

            _reactionService.DeleteReaction(reactionId);
            return Ok("Reaction deleted successfully");
        }
    }
}
