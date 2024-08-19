using ForumProject.Exceptions;
using ForumProject.Helpers;
using ForumProject.Models;
using ForumProject.Models.DTOs;
using ForumProject.Repositories.Contracts;
using ForumProject.Resources;
using ForumProject.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;

public class JWTAuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    private readonly IModelMapper _modelMapper;
    private readonly IAuthManager _authManager;

    public JWTAuthService(IConfiguration configuration, IUserRepository userRepository, IModelMapper modelMapper, IAuthManager authManager)
    {
        _configuration = configuration;
        _userRepository = userRepository;
        _modelMapper = modelMapper;
        _authManager = authManager;
    }

    public UserResponseDto Authenticate(string credentials)
    {
        if (credentials == null || !credentials.Contains(":"))
            throw new InvalidCredentialException(string.Format(ErrorMessages.InvalidCredentialsFormat, credentials));
        string[] parts = credentials.Split(':');
        var username = parts[0];
        var password = parts[1];
        User user;
        try
        {
            user = _userRepository.GetUserByUsername(username);
        }
        catch (EntityNotFoundException)
        {
            throw new InvalidCredentialException(string.Format(ErrorMessages.InvalidCredentialsFormat, credentials));
        }

        if (!_authManager.VerifyPassword(username, password))
            throw new InvalidCredentialException(string.Format(ErrorMessages.InvalidCredentialsFormat, credentials));

        return _modelMapper.ToUserResponseDto(user);
    }

    public string GenerateToken(UserResponseDto user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Username),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username)
    };

        if (user.IsAdmin)
        {
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            if (!string.IsNullOrEmpty(user.PhoneNumber))
            {
                claims.Add(new Claim("PhoneNumber", user.PhoneNumber));
            }
        }

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(double.Parse(jwtSettings["ExpireMinutes"])),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }



    public UserResponseDto Register(UserRequestDto userRequest)
    {
        var user = _modelMapper.ToUser(userRequest);
        _userRepository.RegisterUser(user);
        var userResp = _userRepository.GetUserByUsername(userRequest.Username);

        return _modelMapper.ToUserResponseDto(userResp);
    }

    public bool ValidateToken(string token)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidateAudience = true,
                ValidAudience = jwtSettings["Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return true;
        }
        catch
        {
            return false;
        }
    }
}
