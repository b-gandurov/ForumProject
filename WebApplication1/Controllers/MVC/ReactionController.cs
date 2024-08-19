using ForumProject.Helpers;
using ForumProject.Models.DTOs;
using ForumProject.Models.Enums;
using ForumProject.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ForumProject.Controllers.MVC
{
    public class ReactionController : BaseController
    {
        private readonly IModelMapper _modelMapper;
        private readonly IReactionService _reactionService;
        private readonly IUserService _usersService;
        private readonly IUserContextService _userContextService;

        public ReactionController(
            IModelMapper modelMapper,
            IReactionService reactionService,
            IUserService usersService,
            IUserContextService userContextService) : base(userContextService)
        {
            _modelMapper = modelMapper;
            _reactionService = reactionService;
            _usersService = usersService;
            _userContextService = userContextService;
        }


        [HttpPost]
        [RequireAuthorization]
        public IActionResult AddReaction(int ReactionType, int postId, int? commentId)
        {
            
            try
            {
                var currentUser = _userContextService.GetCurrentUser();
                var user = _usersService.GetUserByUsername(currentUser.Username);

                _reactionService.AddReaction(new ReactionRequestDto()
                {
                    ReactionType = (ReactionType)ReactionType,
                    PostId = postId,
                    CommentId = commentId,
                    UserId = user.Id
                });

                return RedirectToAction("Details", "Posts", new { id = postId });

            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
