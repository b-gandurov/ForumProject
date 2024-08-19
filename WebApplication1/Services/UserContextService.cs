using ForumProject.Models;
using ForumProject.Services.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ForumProject.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public UserContextService(IHttpContextAccessor httpContextAccessor, IAuthService authService, IUserService userService)
        {
            _httpContextAccessor = httpContextAccessor;
            _authService = authService;
            _userService = userService;
        }

        public User GetCurrentUser()
        {
            var context = _httpContextAccessor.HttpContext;
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token == null)
            {
                token = context.Request.Cookies["jwt"];
            }

            if (token == null || !_authService.ValidateToken(token))
            {
                return null;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
            {
                return null;
            }

            var usernameClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub)
                                ?? jwtToken.Claims.FirstOrDefault(claim => claim.Type == "unique_name")
                                ?? jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name);

            if (usernameClaim == null)
            {
                return null;
            }

            var username = usernameClaim.Value;
            var user = _userService.GetUserByUsername(username);

            return user;
        }
    }
}
