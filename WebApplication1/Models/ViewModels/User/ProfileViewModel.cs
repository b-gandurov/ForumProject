namespace ForumProject.Models.ViewModels.User
{
    public class ProfileViewModel
    {
        public UserViewModel User { get; set; }

        public IEnumerable<PostViewModel> Posts { get; set; }

        public bool IsDeleted { get; set; }
    }
}
