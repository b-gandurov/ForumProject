namespace ForumProject.Services.Contracts
{
    public interface ICloudinaryService
    {
        public string UploadProfilePicture(IFormFile file);

        public string UploadPostImage(IFormFile file);
    }
}
