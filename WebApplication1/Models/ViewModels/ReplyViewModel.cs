namespace ForumProject.Models.ViewModels
{
    public class ReplyViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int PostId { get; set; }
        public int? ParentCommentId { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedAt { get; set; }
        public List<ReactionViewModel> Reactions { get; set; } = new List<ReactionViewModel>();
    }
}
