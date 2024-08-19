using Microsoft.AspNetCore.Mvc;
using ForumProject.Helpers;
using ForumProject.Models.DTOs;
using ForumProject.Models.QueryParameters;
using ForumProject.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ForumProject.Services;

namespace ForumProject.Controllers.API
{
    /// <summary>
    /// Controller for managing User-related operations.
    /// </summary>
    [ApiController]
    [Route("api/users")]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IModelMapper _modelMapper;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IUserContextService _userContextService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userService">Service for User operations.</param>
        /// <param name="modelMapper">Model mapper for converting entities and DTOs.</param>
        public UsersController(IUserService userService, 
            IModelMapper modelMapper, 
            ICloudinaryService cloudinaryService,
            IUserContextService userContextService) : base(userContextService)
        {
            _userService = userService;
            _modelMapper = modelMapper;
            _cloudinaryService = cloudinaryService;
            _userContextService = userContextService;
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>List of all users.</returns>
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            var userDtos = users.Select(u => _modelMapper.ToUserResponseDto(u));
            return Ok(userDtos);
        }

        /// <summary>
        /// Gets a User by their username.
        /// </summary>
        /// <param name="username">The username of the User to retrieve.</param>
        /// <returns>User data transfer object.</returns>
        [HttpGet("{username}")]
        public IActionResult GetUserByUsername(string username)
        {
            var user = _userService.GetUserByUsername(username);
            var userDto = _modelMapper.ToUserResponseDto(user);
            return Ok(userDto);
        }

        /// <summary>
        /// Searches users by specified parameters.
        /// </summary>
        /// <param name="parameters">The query parameters for searching users.</param>
        /// <returns>List of users that match the search criteria.</returns>
        [HttpGet("search")]
        public IActionResult SearchUsersBy([FromQuery] UserQueryParameters parameters)
        {
            var users = _userService.SearchUsersBy(parameters);
            var userDtos = users.Select(u => _modelMapper.ToUserResponseDto(u));
            return Ok(userDtos);
        }

        /// <summary>
        /// Updates a User by their ID.
        /// </summary>
        /// <param name="userId">The ID of the User to update.</param>
        /// <param name="userDto">The data transfer object containing updated User details.</param>
        /// <param name="credentials">The credentials of the User making the request.</param>
        /// <returns>Confirmation message of successful update.</returns>
        [HttpPut("{userId}")]
        [RequireAuthorization]
        public IActionResult UpdateUser(int userId, [FromBody] UserRequestDto userDto)
        {
            var userToUpdate = _modelMapper.ToUser(userDto);
            userToUpdate.Id = userId;
            _userService.UpdateUser(userToUpdate);
            return Ok("User updated successfully");
        }

        /// <summary>
        /// Deletes a User by their ID.
        /// </summary>
        /// <param name="userId">The ID of the User to delete.</param>
        /// <param name="credentials">The credentials of the User making the request.</param>
        /// <returns>Confirmation message of successful deletion.</returns>
        [HttpDelete("{userId}")]
        [RequireAuthorization]
        public IActionResult DeleteUser(int userId)
        {
            _userService.DeleteUser(userId);
            return Ok("User deleted successfully");
        }

    }
}
