using System.Security.Authentication;
using ForumProject.Helpers;
using ForumProject.Models;
using ForumProject.Models.DTOs;
using ForumProject.Repositories.Contracts;
using ForumProject.Resources;
using ForumProject.Services.Contracts;
using ForumProject.Exceptions;

namespace ForumProject.Services
{
    /// <summary>
    /// Service for handling authentication-related operations.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthManager _authManager;
        private readonly IModelMapper _modelMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="userRepository">Repository for User operations.</param>
        /// <param name="authManager">Manager for authentication operations.</param>
        /// <param name="modelMapper">Model mapper for converting entities and DTOs.</param>
        public AuthService(IUserRepository userRepository, IAuthManager authManager, IModelMapper modelMapper)
        {
            _userRepository = userRepository;
            _authManager = authManager;
            _modelMapper = modelMapper;
        }

        /// <summary>
        /// Authenticates a User based on provided credentials.
        /// </summary>
        /// <param name="credentials">The credentials of the User in the format "username:password".</param>
        /// <returns>User response data transfer object.</returns>
        /// <exception cref="InvalidCredentialException">Thrown when the credentials are invalid.</exception>
        public UserResponseDto Authenticate(string credentials)
        {
            if (credentials == null || !credentials.Contains(":"))
                throw new InvalidCredentialException(string.Format(ErrorMessages.InvalidCredentialsFormat, credentials));
            string[] parts = credentials.Split(':');
            var username = parts[0];
            var password = parts[1];
            var user = _userRepository.GetUserByUsername(username);
            if (!_authManager.VerifyPassword(username, password))
                throw new InvalidCredentialException(string.Format(ErrorMessages.IncorrectCredentials, username, password));

            return _modelMapper.ToUserResponseDto(user);
        }

        /// <summary>
        /// Registers a new User.
        /// </summary>
        /// <param name="userRequest">The data transfer object containing User registration details.</param>
        /// <returns>User response data transfer object.</returns>
        /// <exception cref="DuplicateEntityException">Thrown when the username or email is already taken.</exception>
        public UserResponseDto Register(UserRequestDto userRequest)
        {
            if (_userRepository.UserExists(userRequest.Username))
                throw new DuplicateEntityException(string.Format(ErrorMessages.UsernameTaken, userRequest.Username));
            if (_userRepository.EmailExists(userRequest.Email))
                throw new DuplicateEntityException(string.Format(ErrorMessages.EmailTaken, userRequest.Email));
            var user = _modelMapper.ToUser(userRequest);
            _userRepository.RegisterUser(user);
            var userResp = _userRepository.GetUserByUsername(userRequest.Username);

            return _modelMapper.ToUserResponseDto(userResp);
        }

        /// <summary>
        /// Generates a token for a User.
        /// </summary>
        /// <param name="user">The User for whom to generate the token.</param>
        /// <returns>A token string.</returns>
        public string GenerateToken(UserResponseDto user)
        {
            throw new NotImplementedException();
        }

        public bool ValidateToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
