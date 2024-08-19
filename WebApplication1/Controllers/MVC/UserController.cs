using ForumProject.Exceptions;
using ForumProject.Helpers;
using ForumProject.Models;
using ForumProject.Models.QueryParameters;
using ForumProject.Models.ViewModels;
using ForumProject.Models.ViewModels.User;
using ForumProject.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ForumProject.Controllers.MVC
{
    public class UserController : BaseController
    {
        private readonly IPostService _postService;
        private readonly IModelMapper _modelMapper;
        private readonly IUserService _usersService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IUserContextService _userContextService;

        public UserController(IPostService postService,
            IModelMapper modelMapper,
            IUserService usersService,
            ICloudinaryService cloudinaryService,
            IUserContextService userContextService) : base(userContextService)
        {
            _postService = postService;
            _modelMapper = modelMapper;
            _usersService = usersService;
            _cloudinaryService = cloudinaryService;
            _userContextService = userContextService;
        }


        [RequireAuthorization]
        public IActionResult Index()
        {
            var currentUser = _userContextService.GetCurrentUser();

            var user = _usersService.GetUserByUsername(currentUser.Username);

            ViewBag.IsAdmin = currentUser.IsAdmin;
            ViewBag.AdminPhone = user.PhoneNumber;

            UserViewModel userVM = _modelMapper.ToUserViewModel(user);

            return View(new UserIndexPageViewModel()
            {
                User = userVM,
            });
        }

        //CRUD

        //Register is for Create - is in Authentication

        public IActionResult Search(string username) //Read
        {
            if (string.IsNullOrEmpty(username))
            {
                return View(new SearchUserViewModel()
                {
                    Result = new List<UserViewModel>(),
                });
            }

            var users = _usersService.SearchUsersBy(new UserQueryParameters()
            {
                Username = username
            });

            IEnumerable<UserViewModel> usersVM = users.Select(x => _modelMapper.ToUserViewModel(x));

            var response = new SearchUserViewModel()
            {
                Result = usersVM,
            };

            return View(response);
        }
        [RequireAuthorization(checkIfBlocked: true)]
        public IActionResult Profile(string username) //Read
        {
            try
            {
                var user = _usersService.GetUserByUsername(username);
                var currentUser = _userContextService.GetCurrentUser();

                UserViewModel userVM = _modelMapper.ToUserViewModel(user);

                var posts = _postService.GetPostsByUserId(user.Id);

                IEnumerable<PostViewModel> postsVM = posts.Select(x => _modelMapper.ToPostViewModel(x));

                ViewBag.IsAdmin = currentUser.IsAdmin;
                ViewBag.IsBlocked = user.IsBlocked;
                ViewBag.IsUserAdmin = user.IsAdmin;

                return View(new ProfileViewModel()
                {
                    User = userVM,
                    Posts = postsVM,
                    IsDeleted = user.DeletedAt == null ? false : true,
                });
            }
            catch (EntityNotFoundException)
            {
                return View(new ProfileViewModel()
                {
                    IsDeleted = true,
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [RequireAuthorization]
        public IActionResult Update(int userId) //Update
        {
            var currentUser = _userContextService.GetCurrentUser();

            var user = _usersService.GetUserById(userId);

            if (!currentUser.Username.Equals(user.Username))
            {
                return BadRequest("Can't update this User.");
            }

            UserViewModel userVM = _modelMapper.ToUserViewModel(user);

            ViewBag.UserId = user.Id;

            return View(new UpdateUserViewModel()
            {
                Email = userVM.Email,
                FirstName = userVM.FirstName,
                LastName = userVM.LastName,
                PhoneNumber = user?.PhoneNumber,
                IsAdmin = currentUser.IsAdmin,
                UserId = userId,
            });
        }

        [HttpPost]
        [RequireAuthorization]
        public IActionResult Update(UpdateUserViewModel updateUserViewModel) //Update
        {
            if (!ModelState.IsValid)
            {
                return View(updateUserViewModel);
            }

            try
            {
                User userToUpdate = new User()
                {
                    Id = updateUserViewModel.UserId,
                    Email = updateUserViewModel.Email,
                    FirstName = updateUserViewModel.FirstName,
                    LastName = updateUserViewModel.LastName,
                    PhoneNumber = updateUserViewModel.PhoneNumber,
                };


                if (!string.IsNullOrEmpty(updateUserViewModel.NewPassword) && updateUserViewModel.NewPassword != updateUserViewModel.NewPasswordConfirmation)
                {
                    throw new Exception("Passwords don't match");
                }

                if (!string.IsNullOrEmpty(updateUserViewModel.NewPassword) && updateUserViewModel.NewPassword == updateUserViewModel.NewPasswordConfirmation)
                {
                    userToUpdate.Password = updateUserViewModel.NewPassword;
                }
                _usersService.UpdateUser(userToUpdate);
                return RedirectToAction("");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return View(updateUserViewModel);
            }
            

            
        }

        [HttpPost]
        public IActionResult Upload(IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                var img = _cloudinaryService.UploadProfilePicture(image);
                User user = ViewBag.CurrentUser;
                user.ProfilePictureUrl = img;
                _usersService.UpdateUser(user);
                return RedirectToAction("Index","User");
            }
            else
            {
                TempData["ErrorMessage"] = "Please upload a file!";
                return RedirectToAction("Index", "User");
            }
        }



        [RequireAuthorization]
        public IActionResult Delete(int userId) //Update
        {
            var currentUser = _userContextService.GetCurrentUser();

            var user = _usersService.GetUserById(userId);

            if (!currentUser.Username.Equals(user.Username))
            {
                return BadRequest("Can't update this user.");
            }

            UserViewModel userVM = _modelMapper.ToUserViewModel(user);

            ViewBag.UserId = user.Id;

            return View(new UpdateUserViewModel()
            {
                Email = userVM.Email,
                FirstName = userVM.FirstName,
                LastName = userVM.LastName,
                PhoneNumber = user?.PhoneNumber,
                IsAdmin = currentUser.IsAdmin,
                UserId = userId,
            });
        }

        [HttpPost, ActionName("Delete")]
        [RequireAuthorization]
        public IActionResult ConfirmDelete(int userId)
        {
            _usersService.DeleteUser(userId);
            HttpContext.Response.Cookies.Delete("jwt");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [RequireAuthorization]
        public IActionResult BlockUser(int userId)
        {
            var currentUser = _userContextService.GetCurrentUser();

            if (currentUser.IsAdmin)
            {
                _usersService.BlockUser(userId);
            }
            else
            {
                return BadRequest("You can't block this User");
            }

            var updatedUser = _usersService.GetUserById(userId);
            return RedirectToAction("Profile", "User", new { username = updatedUser.Username });
        }

        [HttpPost]
        [RequireAuthorization]
        public IActionResult UnblockUser(int userId)
        {
            var currentUser = _userContextService.GetCurrentUser();

            if (currentUser.IsAdmin)
            {
                _usersService.UnblockUser(userId);
            }
            else
            {
                return BadRequest("You can't unblock this user");
            }

            var updatedUser = _usersService.GetUserById(userId);
            return RedirectToAction("Profile", "User", new { username = updatedUser.Username });
        }

        [HttpPost]
        [RequireAuthorization]
        public IActionResult DemoteFromAdmin(int userId)
        {
            var currentUser = _userContextService.GetCurrentUser();

            if (currentUser.IsAdmin)
            {
                _usersService.DemoteUserFromAdmin(userId);
            }
            else
            {
                return BadRequest("You can't demote this user");
            }

            var updatedUser = _usersService.GetUserById(userId);
            return RedirectToAction("Profile", "User", new { username = updatedUser.Username });
        }

        [HttpPost]
        [RequireAuthorization]
        public IActionResult PromoteToAdmin(int userId)
        {
            var currentUser = _userContextService.GetCurrentUser();

            if (currentUser.IsAdmin)
            {
                _usersService.PromoteUserToAdmin(userId);
            }
            else
            {
                return BadRequest("You can't promote this user");
            }

            var updatedUser = _usersService.GetUserById(userId);
            return RedirectToAction("Profile", "User", new { username = updatedUser.Username });
        }
    }
}
