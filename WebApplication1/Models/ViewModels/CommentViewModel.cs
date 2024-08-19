using System.ComponentModel.DataAnnotations;
namespace ForumProject.Models.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; }
        public int PostId { get; set; }
        public int? ParentCommentId { get; set; }

        public UserViewModel? User { get; set; }
        //public string UserName { get; set; }
        //public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedAt { get; set; }
        public List<ReactionViewModel> Reactions { get; set; } = new List<ReactionViewModel>();
        public List<CommentViewModel> Replies { get; set; } = new List<CommentViewModel>();

        public Dictionary<ReactionViewModel, int> ReactionCounts
        {
            get
            {
                if (Reactions == null)
                {
                    return new Dictionary<ReactionViewModel, int>();
                }

                var reactionViewModels = Reactions.Select(r => new ReactionViewModel
                {
                    ReactionType = r.ReactionType,
                    Emoji = r.Emoji
                });

                return reactionViewModels
                    .GroupBy(r => r.ReactionType)
                    .ToDictionary(g => g.First(), g => g.Count());
            }
        }
    }
}

