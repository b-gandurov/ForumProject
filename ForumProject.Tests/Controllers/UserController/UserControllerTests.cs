using ForumProject.Helpers;
using ForumProject.Models;
using ForumProject.Models.QueryParameters;
using ForumProject.Models.ViewModels;
using ForumProject.Models.ViewModels.User;
using ForumProject.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ForumProject.Tests.Controllers.UserController
{
    [TestClass]
    public class UserControllerTests
    {
        private Mock<IPostService> _mockPostService;
        private Mock<IModelMapper> _mockModelMapper;
        private Mock<IUserService> _mockUserService;
        private Mock<ICloudinaryService> _mockCloudinaryService;
        private Mock<IUserContextService> _mockUserContextService;
        private ForumProject.Controllers.MVC.UserController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _mockPostService = new Mock<IPostService>();
            _mockModelMapper = new Mock<IModelMapper>();
            _mockUserService = new Mock<IUserService>();
            _mockCloudinaryService = new Mock<ICloudinaryService>();
            _mockUserContextService = new Mock<IUserContextService>();

            _controller = new ForumProject.Controllers.MVC.UserController(
                _mockPostService.Object,
                _mockModelMapper.Object,
                _mockUserService.Object,
                _mockCloudinaryService.Object,
                _mockUserContextService.Object
            );
        }

        [TestMethod]
        public void Index_ShouldReturnView_WithUserAndAdminInfo()
        {
            // Arrange
            var currentUser = new User { Username = "testuser", IsAdmin = true };
            var user = new User { PhoneNumber = "1234567890" };
            _mockUserContextService.Setup(s => s.GetCurrentUser()).Returns(currentUser);
            _mockUserService.Setup(s => s.GetUserByUsername("testuser")).Returns(user);
            _mockModelMapper.Setup(m => m.ToUserViewModel(user)).Returns(new UserViewModel());

            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as UserIndexPageViewModel;
        }

        [TestMethod]
        public void Search_ShouldReturnView_WithSearchResults()
        {
            // Arrange
            var username = "searchuser";
            var users = new List<User> { new User { Username = username } };
            _mockUserService.Setup(s => s.SearchUsersBy(It.Is<UserQueryParameters>(p => p.Username == username)))
                .Returns(users);
            _mockModelMapper.Setup(m => m.ToUserViewModel(It.IsAny<User>())).Returns(new UserViewModel());

            // Act
            var result = _controller.Search(username) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as SearchUserViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Result.Count());
        }

        [TestMethod]
        public void Profile_ShouldReturnView_WithUserProfile()
        {
            // Arrange
            var username = "testuser";
            var user = new User { Username = username, IsBlocked = false, DeletedAt = null };
            var posts = new List<Post>();
            _mockUserService.Setup(s => s.GetUserByUsername(username)).Returns(user);
            _mockPostService.Setup(s => s.GetPostsByUserId(user.Id)).Returns(posts);
            _mockModelMapper.Setup(m => m.ToUserViewModel(user)).Returns(new UserViewModel());
            _mockModelMapper.Setup(m => m.ToPostViewModel(It.IsAny<Post>())).Returns(new PostViewModel());
            _mockUserContextService.Setup(s => s.GetCurrentUser()).Returns(user);

            // Act
            var result = _controller.Profile(username) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as ProfileViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(posts.Count, model.Posts.Count());
            Assert.IsFalse(model.IsDeleted);
        }

        [TestMethod]
        public void Update_ShouldReturnView_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            var currentUser = new User { Username = "currentuser" };
            var user = new User { Id = userId, Username = "currentuser" };
            _mockUserContextService.Setup(s => s.GetCurrentUser()).Returns(currentUser);
            _mockUserService.Setup(s => s.GetUserById(userId)).Returns(user);
            _mockModelMapper.Setup(m => m.ToUserViewModel(user)).Returns(new UserViewModel());

            // Act
            var result = _controller.Update(userId) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as UpdateUserViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(userId, model.UserId);
        }

        [TestMethod]
        public void UpdatePost_ShouldRedirectToHome_WhenValidModel()
        {
            // Arrange
            var viewModel = new UpdateUserViewModel
            {
                UserId = 1,
                Email = "email@example.com",
                FirstName = "First",
                LastName = "Last",
                PhoneNumber = "1234567890",
                NewPassword = "newpassword",
                NewPasswordConfirmation = "newpassword"
            };
            _mockUserService.Setup(s => s.UpdateUser(It.IsAny<User>()));

            // Act
            var result = _controller.Update(viewModel) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
        }

        [TestMethod]
        public void Upload_ShouldRedirectToUser_WhenFileIsUploaded()
        {
            // Arrange
            var imageFile = new Mock<IFormFile>();
            imageFile.Setup(f => f.Length).Returns(100);
            var uploadedImageUrl = "http://example.com/image.jpg";
            _mockCloudinaryService.Setup(s => s.UploadProfilePicture(imageFile.Object)).Returns(uploadedImageUrl);
            var user = new User();
            _controller.ViewBag.CurrentUser = user;

            // Act
            var result = _controller.Upload(imageFile.Object) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("User", result.ControllerName);
        }

        [TestMethod]
        public void BlockUser_ShouldRedirectToProfile_WhenAdmin()
        {
            // Arrange
            var userId = 1;
            var currentUser = new User { IsAdmin = true };
            var updatedUser = new User { Username = "updateduser" };
            _mockUserContextService.Setup(s => s.GetCurrentUser()).Returns(currentUser);
            _mockUserService.Setup(s => s.BlockUser(userId));
            _mockUserService.Setup(s => s.GetUserById(userId)).Returns(updatedUser);

            // Act
            var result = _controller.BlockUser(userId) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Profile", result.ActionName);
            Assert.AreEqual("User", result.ControllerName);
            Assert.AreEqual(updatedUser.Username, result.RouteValues["username"]);
        }

        [TestMethod]
        public void UnblockUser_ShouldRedirectToProfile_WhenAdmin()
        {
            // Arrange
            var userId = 1;
            var currentUser = new User { IsAdmin = true };
            var updatedUser = new User { Username = "updateduser" };
            _mockUserContextService.Setup(s => s.GetCurrentUser()).Returns(currentUser);
            _mockUserService.Setup(s => s.UnblockUser(userId));
            _mockUserService.Setup(s => s.GetUserById(userId)).Returns(updatedUser);

            // Act
            var result = _controller.UnblockUser(userId) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Profile", result.ActionName);
            Assert.AreEqual("User", result.ControllerName);
            Assert.AreEqual(updatedUser.Username, result.RouteValues["username"]);
        }

        [TestMethod]
        public void DemoteFromAdmin_ShouldRedirectToProfile_WhenAdmin()
        {
            // Arrange
            var userId = 1;
            var currentUser = new User { IsAdmin = true };
            var updatedUser = new User { Username = "updateduser" };
            _mockUserContextService.Setup(s => s.GetCurrentUser()).Returns(currentUser);
            _mockUserService.Setup(s => s.DemoteUserFromAdmin(userId));
            _mockUserService.Setup(s => s.GetUserById(userId)).Returns(updatedUser);

            // Act
            var result = _controller.DemoteFromAdmin(userId) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Profile", result.ActionName);
            Assert.AreEqual("User", result.ControllerName);
            Assert.AreEqual(updatedUser.Username, result.RouteValues["username"]);
        }

        [TestMethod]
        public void PromoteToAdmin_ShouldRedirectToProfile_WhenAdmin()
        {
            // Arrange
            var userId = 1;
            var currentUser = new User { IsAdmin = true };
            var updatedUser = new User { Username = "updateduser" };
            _mockUserContextService.Setup(s => s.GetCurrentUser()).Returns(currentUser);
            _mockUserService.Setup(s => s.PromoteUserToAdmin(userId));
            _mockUserService.Setup(s => s.GetUserById(userId)).Returns(updatedUser);

            // Act
            var result = _controller.PromoteToAdmin(userId) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Profile", result.ActionName);
            Assert.AreEqual("User", result.ControllerName);
            Assert.AreEqual(updatedUser.Username, result.RouteValues["username"]);
        }
    }
}
