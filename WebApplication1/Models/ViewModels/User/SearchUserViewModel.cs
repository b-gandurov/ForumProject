namespace ForumProject.Models.ViewModels.User
{
    public class SearchUserViewModel
    {
        public string Username { get; set; }

        public IEnumerable<UserViewModel> Result { get; set; }
    }
}
