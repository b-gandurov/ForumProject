using Microsoft.AspNetCore.Mvc;
using ForumProject.Helpers;
using ForumProject.Models.DTOs;
using ForumProject.Services.Contracts;

namespace ForumProject.Controllers.API
{
    /// <summary>
    /// Admin controller for managing User-related administrative tasks.
    /// Requires admin authorization.
    /// </summary>
    [ApiController]
    [Route("api/admin")]
    [RequireAuthorization(requireAdmin: true)]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IModelMapper _modelMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminController"/> class.
        /// </summary>
        /// <param name="userService">Service for User operations.</param>
        /// <param name="authService">Service for authentication operations.</param>
        /// <param name="modelMapper">Model mapper for converting entities and DTOs.</param>
        public AdminController(IUserService userService, IAuthService authService, IModelMapper modelMapper)
        {
            _userService = userService;
            _authService = authService;
            _modelMapper = modelMapper;
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>List of all users.</returns>
        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            var userDtos = users.Select(u => _modelMapper.ToUserResponseDto(u));
            return Ok(userDtos);
        }

        /// <summary>
        /// Registers a new User.
        /// </summary>
        /// <param name="userDto">User data transfer object containing User details.</param>
        /// <returns>Confirmation message of successful registration.</returns>
        [HttpPost("addUser")]
        public IActionResult Register([FromBody] UserRequestDto userDto)
        {
            _authService.Register(userDto);
            return Ok(new { message = $"User {userDto.Username} registered successfully" });
        }

        /// <summary>
        /// Gets a User by their ID.
        /// </summary>
        /// <param name="userId">The ID of the User to retrieve.</param>
        /// <returns>User data transfer object.</returns>
        [HttpGet("users/{userId}")]
        public IActionResult GetUserById(int userId)
        {
            var user = _userService.GetUserById(userId);
            var userDto = _modelMapper.ToUserResponseDto(user);
            return Ok(userDto);
        }

        /// <summary>
        /// Blocks a User by their ID.
        /// </summary>
        /// <param name="userId">The ID of the User to block.</param>
        /// <returns>Confirmation message of successful blocking.</returns>
        [HttpPut("users/{userId}/block")]
        public IActionResult BlockUser(int userId)
        {
            _userService.BlockUser(userId);
            return Ok(new { message = "User blocked successfully" });
        }

        /// <summary>
        /// Unblocks a User by their ID.
        /// </summary>
        /// <param name="userId">The ID of the User to unblock.</param>
        /// <returns>Confirmation message of successful unblocking.</returns>
        [HttpPut("users/{userId}/unblock")]
        public IActionResult UnblockUser(int userId)
        {
            _userService.UnblockUser(userId);
            return Ok(new { message = "User unblocked successfully" });
        }

        /// <summary>
        /// Deletes a User by their ID.
        /// </summary>
        /// <param name="userId">The ID of the User to delete.</param>
        /// <returns>Confirmation message of successful deletion.</returns>
        [HttpDelete("users/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            _userService.DeleteUser(userId);
            return Ok(new { message = "User deleted successfully" });
        }

        /// <summary>
        /// Promotes a User to admin by their ID.
        /// </summary>
        /// <param name="userId">The ID of the User to promote.</param>
        /// <returns>Confirmation message of successful promotion.</returns>
        [HttpPut("users/{userId}/promote")]
        public IActionResult PromoteToAdmin(int userId)
        {
            _userService.PromoteUserToAdmin(userId);
            return Ok(new { message = "User promoted to admin successfully" });
        }

        /// <summary>
        /// Demotes a User from admin by their ID.
        /// </summary>
        /// <param name="userId">The ID of the User to demote.</param>
        /// <returns>Confirmation message of successful demotion.</returns>
        [HttpPut("users/{userId}/demote")]
        public IActionResult DemoteFromAdmin(int userId)
        {
            _userService.DemoteUserFromAdmin(userId);
            return Ok(new { message = "User demoted from admin successfully" });
        }
    }
}
