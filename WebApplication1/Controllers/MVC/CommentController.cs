using Microsoft.AspNetCore.Mvc;
using ForumProject.Controllers;
using ForumProject.Models;
using ForumProject.Services.Contracts;
using System;
using ForumProject.Exceptions;
using ForumProject.Helpers;
using CloudinaryDotNet.Actions;
using ForumProject.Models.ViewModels;

namespace ForumProject.Controllers.MVC
{
    public class CommentController : BaseController
    {
        private readonly ICommentService _commentService;
        private readonly IModelMapper _modelMapper;
        private readonly IUserService _usersService;

        public CommentController(
            ICommentService commentService, 
            IUserContextService userContextService, 
            IModelMapper modelMapper, 
            IUserService usersService) : base(userContextService)
        {
            _commentService = commentService;
            _modelMapper = modelMapper;
            _usersService = usersService;
        }


        [HttpPost]
        [RequireAuthorization(checkIfBlocked: true)]
        public IActionResult Create(CommentViewModel commentViewModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return RedirectToAction("Details", "Posts", new { id = commentViewModel.PostId });
            }
            try
            {
                var user = ViewBag.CurrentUser;
                commentViewModel.User = _modelMapper.ToUserViewModel(user);
                var newComment = _modelMapper.ToComment(commentViewModel);
                newComment.User = user;
                newComment.UserId = user.Id;
                _commentService.AddComment(newComment);
                return RedirectToAction("Details", "Posts", new { id = newComment.PostId });
            }
            catch (DuplicateEntityException ex)
            {
                ModelState.AddModelError("Content", ex.Message);
                return RedirectToAction("Details", "Posts", new { id = commentViewModel.PostId });
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                Response.StatusCode = 500;
                return View("Error");
            }
        }

        [HttpGet]
        [RequireAuthorization(checkIfBlocked: true)]
        public IActionResult Edit(int id)
        {
            var comment = _commentService.GetCommentById(id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }

        [HttpPost]
        [RequireAuthorization(checkIfBlocked: true)]
        public IActionResult Edit(CommentViewModel commentViewModel)
        {
            if (!ModelState.IsValid)
            {
                // Redirect back to the post details page with the current model to display errors
                return RedirectToAction("Details", "Posts", new { id = commentViewModel.PostId });
            }

            var user = HttpContext.Items["User"] as User;

            try
            {
                var comment = _modelMapper.ToComment(commentViewModel);
                comment.User = user;
                comment.UserId = user.Id;
                _commentService.UpdateComment(comment);
                return RedirectToAction("Details", "Posts", new { id = comment.PostId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Details", "Posts", new { id = commentViewModel.PostId });
            }
        }

        [HttpGet]
        [RequireAuthorization]
        public IActionResult Delete(int id)
        {
            var comment = _commentService.GetCommentById(id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }

        [HttpPost, ActionName("Delete")]
        [RequireAuthorization]
        public IActionResult DeleteConfirmed(int id)
        {
            var comment = _commentService.GetCommentById(id);
            var postId = comment.PostId;
            _commentService.DeleteComment(id);
            return RedirectToAction("Details", "Posts", new { id = postId });
        }
    }
}
