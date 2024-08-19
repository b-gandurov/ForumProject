using ForumProject.Models.Enums;
using ForumProject.Models.ViewModels;
using System.Collections.Generic;

public static class ReactionHelper
{
    public static List<ReactionViewModel> GetReactions()
    {
        var reactions = new List<ReactionViewModel>();

        foreach (var reactionType in (ReactionType[])Enum.GetValues(typeof(ReactionType)))
        {
            reactions.Add(new ReactionViewModel
            {
                ReactionType = reactionType.ToString(),
                Emoji = GetEmoji(reactionType)
            });
        }

        return reactions;
    }

    private static string GetEmoji(ReactionType reactionType)
    {
        return reactionType switch
        {
            ReactionType.Like => "❤️",
            ReactionType.Dislike => "👎",
            ReactionType.Knitting => "🧶",
            ReactionType.Food=> "🍲",
            ReactionType.Gossip=> "🙊",
            ReactionType.Glasses=> "👓",
            ReactionType.Laugh=> "😄",
            _ => ""
        };
    }
}
