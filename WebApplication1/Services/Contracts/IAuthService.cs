using ForumProject.Models;
using ForumProject.Models.DTOs;

namespace ForumProject.Services.Contracts
{
    public interface IAuthService
    {
        public UserResponseDto Register(UserRequestDto userRequest);
        UserResponseDto Authenticate(string credentials);

        string GenerateToken(UserResponseDto user);
        bool ValidateToken(string token);
    }
}
