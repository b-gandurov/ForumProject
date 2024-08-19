namespace ForumProject.Models.ViewModels.User
{
    public class UserIndexPageViewModel
    {
        public UserViewModel User { get; set; }
        public SearchUserViewModel SearchUserModel { get; set; }

        public UserIndexPageViewModel()
        {
            SearchUserModel = new SearchUserViewModel()
            {
                Result = new List<UserViewModel>()
            };
        }
    }
}
