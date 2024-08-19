using ForumProject.Models.Enums;

namespace ForumProject.Models.ViewModels
{
    public class ReactionViewModel
    {
        public int Id { get; set; }
        public int? CommentId { get; set; }

        public int? PostId { get; set; }
        public string ReactionType { get; set; }
        public string UserName { get; set; }

        public int Count { get; set; }

        public string Emoji { get; set; }

        public int ReactionTypeId
        {
            get
            {
                if (Enum.TryParse(typeof(ReactionType), ReactionType, out var reactionTypeEnum))
                {
                    return (int)reactionTypeEnum;
                }
                throw new InvalidOperationException($"Invalid ReactionType: {ReactionType}");
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is ReactionViewModel other)
            {
                return string.Equals(ReactionType, other.ReactionType, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(ReactionType);
        }
    }
}
