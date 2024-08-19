using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using ForumProject.Repositories.Contracts;
using ForumProject.Services.Contracts;

public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;
    private readonly IUserRepository _userRepository;

    public CloudinaryService(IConfiguration configuration, IUserRepository userRepository)
    {
        Account account = new Account(
            configuration["CloudinarySettings:CloudName"],
            configuration["CloudinarySettings:ApiKey"],
            configuration["CloudinarySettings:ApiSecret"]
        );

        _cloudinary = new Cloudinary(account);
        _userRepository = userRepository;
    }

    public string UploadProfilePicture(IFormFile file)
    {
        using (var stream = file.OpenReadStream())
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream)
            };

            var uploadResult = _cloudinary.Upload(uploadParams);

            return uploadResult.SecureUrl.ToString();
        }
    }

    public string UploadPostImage(IFormFile file)
    {
        using (var stream = file.OpenReadStream())
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream)
            };

            var uploadResult = _cloudinary.Upload(uploadParams);

            return uploadResult.SecureUrl.ToString();
        }
    }
}
