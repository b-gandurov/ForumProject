using ForumProject.Models.Enums;
using ForumProject.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

public class PostViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]

    [StringLength(64, MinimumLength = 16, ErrorMessage = "Title must be between 16 and 64 characters")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Content is required")]
    [StringLength(8192, MinimumLength = 32, ErrorMessage = "Content must be between 32 and 8192  characters")]
    public string Content { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int UserId { get; set; }
    public string Username { get; set; }

    public ICollection<CommentViewModel>? Comments { get; set; }
    public ICollection<ReactionViewModel>? Reactions { get; set; }
    public string? ImageUrl { get; set; }

    public IFormFile? File { get; set; }

    [Required(ErrorMessage = "Category is required")]
    public PostCategory Category { get; set; }



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
