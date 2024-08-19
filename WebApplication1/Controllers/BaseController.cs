using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ForumProject.Services.Contracts;
using ForumProject.Models.Enums;
using ForumProject.Models.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using ForumProject.Services;

namespace ForumProject.Controllers
{
    public class BaseController : Controller
    {
        private readonly IUserContextService _userContextService;

        public BaseController(IUserContextService userContextService)
        {
            _userContextService = userContextService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var user = _userContextService.GetCurrentUser();
            var userName = user?.Username ?? string.Empty;
            var isAdmin = user?.IsAdmin ?? false;
            ViewBag.Username = userName;
            ViewBag.IsAdmin = isAdmin;
            ViewBag.CurrentUser = user;
            ViewBag.Categories = Enum.GetValues(typeof(PostCategory)).Cast<PostCategory>().ToList();
            ViewBag.IsAuthenticated = user != null;
            ViewBag.Emojis = ReactionHelper.GetReactions();
        }
    }
}
