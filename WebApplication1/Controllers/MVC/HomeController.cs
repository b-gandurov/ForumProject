using ForumProject.Helpers;
using ForumProject.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ForumProject.Controllers.MVC
{
    public class HomeController : BaseController
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;
        private readonly IModelMapper _modelMapper;
        public HomeController(IUserContextService userContextService,IPostService postService, IUserService userService,IModelMapper modelMapper) : base(userContextService)
        {
            _postService = postService;
            _userService = userService;
            _modelMapper = modelMapper;
        }
        public IActionResult Index()
        {
            ViewBag.TopReactedPosts = _postService.GetTopReactedPosts(10).Select(p=> _modelMapper.ToPostViewModel(p)).OrderByDescending(p=>p.Reactions?.Count);
            ViewBag.TopCommentedPosts = _postService.GetTopCommentedPosts(10).OrderByDescending(p=>p.Comments.Count);
            ViewBag.RecentPosts = _postService.GetRecentPosts(10);
            ViewBag.TotalUsers = _userService.GetAllUsers();
            ViewBag.TotalPosts = _postService.GetAllPosts();
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
