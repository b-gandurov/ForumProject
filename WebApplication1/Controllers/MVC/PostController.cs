using ForumProject.Exceptions;
using ForumProject.Helpers;
using ForumProject.Models;
using ForumProject.Models.Enums;
using ForumProject.Models.QueryParameters;
using ForumProject.Models.ViewModels;
using ForumProject.Services;
using ForumProject.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace ForumProject.Controllers.MVC
{
    public class PostsController : BaseController
    {
        private readonly IPostService _postService;
        private readonly IModelMapper _modelMapper;
        private readonly IUserService _usersService;
        private readonly ICloudinaryService _cloudinaryService;

        public PostsController(
            IPostService postService,
            IModelMapper modelMapper,
            IUserService usersService,
            IUserContextService userContextService,
            ICloudinaryService cloudinaryService) : base(userContextService)
        {
            _postService = postService;
            _modelMapper = modelMapper;
            _usersService = usersService;
            _cloudinaryService = cloudinaryService;
        }

        public IActionResult Index([FromQuery] PostQueryParameters filterParameters)
        {
            if (string.IsNullOrEmpty(filterParameters.Category.ToString()))
            {
                filterParameters.Category = null;
            }

            var posts = _postService.FilterBy(filterParameters);
            var postViewModels = posts.Select(p => _modelMapper.ToPostViewModel(p)).ToList();
            var totalCount = _postService.GetTotalCount(filterParameters);

            //ViewBag.FilterParameters = filterParameters;
            ViewBag.TotalCount = totalCount;
            ViewBag.PageSize = filterParameters.PageSize;
            ViewBag.PageNumber = filterParameters.PageNumber;
            ViewBag.Categories = Enum.GetValues(typeof(PostCategory)).Cast<PostCategory>().ToList();

            return View(postViewModels);
        }



        public IActionResult Details(int id)
        {
            try
            {
                var post = _postService.GetPostById(id);
                var postViewModel = _modelMapper.ToPostViewModel(post);

                ViewData["id"] = post.Id;
                return View(postViewModel);
            }
            catch (EntityNotFoundException ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                Response.StatusCode = NotFound().StatusCode;
                return View("Error");
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
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [RequireAuthorization(checkIfBlocked: true)]
        public IActionResult Create(PostViewModel postViewModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
                return View(postViewModel);
            }
            try
            {
                var newPost = _modelMapper.ToPost(postViewModel);
                var user = _usersService.GetUserByUsername(postViewModel.Username);
                newPost.User = user;
                newPost.UserId = user.Id;
                var file = postViewModel.File;
                if (file != null && file.Length > 0)
                {
                    var imageUrl = _cloudinaryService.UploadPostImage(file);
                    newPost.ImageUrl = imageUrl;
                }

                _postService.AddPost(newPost);
                return RedirectToAction("Details", new { id = newPost.Id });
            }
            catch (DuplicateEntityException ex)
            {
                ModelState.AddModelError("Title", ex.Message);
                return View(postViewModel);
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
            var post = _postService.GetPostById(id);
            var postViewModel = _modelMapper.ToPostViewModel(post);

            return View(postViewModel);
        }

        [HttpPost]
        [RequireAuthorization(checkIfBlocked: true)]
        public IActionResult Edit(int id, PostViewModel postViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(postViewModel);
            }
 
            var postToUpdate = _postService.GetPostById(id);
            if (postToUpdate == null)
            {
                throw new EntityNotFoundException("Post not found");
            }

            var file = postViewModel.File;
            if (file != null && file.Length > 0)
            {
                var imageUrl = _cloudinaryService.UploadPostImage(file);
                postToUpdate.ImageUrl = imageUrl;
            }

            postToUpdate.Title = postViewModel.Title;
            postToUpdate.Content = postViewModel.Content;
            postToUpdate.Category = postViewModel.Category;

            _postService.UpdatePost(postToUpdate);
            return RedirectToAction("Details", new { id = postToUpdate.Id });
        }

        [HttpGet]
        [RequireAuthorization]
        public IActionResult Delete(int id)
        {
            var post = _postService.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }
            var postViewModel = _modelMapper.ToPostViewModel(post);
            return View(postViewModel);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [RequireAuthorization]
        public IActionResult DeleteConfirmed(int id)
        {
            var post = _postService.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }
            _postService.DeletePost(id);
            return RedirectToAction("Index");
        }


    }
}