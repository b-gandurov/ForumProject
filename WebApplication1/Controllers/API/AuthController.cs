using Microsoft.AspNetCore.Mvc;
using ForumProject.Models.DTOs;
using ForumProject.Services.Contracts;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ForumProject.Helpers;

namespace ForumProject.Controllers.API
{
    /// <summary>
    /// Controller for handling authentication-related operations.
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IModelMapper _modelMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authService">Service for authentication operations.</param>
        public AuthController(IAuthService authService, IModelMapper mapper)
        {
            _authService = authService;
            _modelMapper = mapper;
        }

        /// <summary>
        /// Registers a new User.
        /// </summary>
        /// <param name="userDto">User data transfer object containing User details.</param>
        /// <returns>Confirmation message of successful registration.</returns>
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRequestDto userDto)
        {
            _authService.Register(userDto);
            return Ok(new { message = $"User {userDto.Username} registered successfully" });
        }

        /// <summary>
        /// Authenticates a User and generates a JWT token.
        /// </summary>
        /// <param name="loginDto">Login data transfer object containing username and password.</param>
        /// <returns>JWT token if authentication is successful.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var credentials = loginRequestDto.Username + ":" + loginRequestDto.Password;
            var user = _authService.Authenticate(credentials);
            var token = _authService.GenerateToken(user);
            return Ok(new { Token = token });
        }
    }
}
