using ForumProject.Models.DTOs;
using ForumProject.Models.QueryParameters;

namespace ForumProject.Repositories.Contracts
{
    public interface IReactionRepository
    {
        ReactionResponseDto GetReactionById(int id);
        void AddReaction(ReactionRequestDto reaction);
        void UpdateReaction(ReactionRequestDto reaction);
        void DeleteReaction(int id);
        IEnumerable<ReactionResponseDto> GetReactionsByPostId(int postId);

        IEnumerable<ReactionResponseDto> GetReactionsByCommentId(int commentId);
        IEnumerable<ReactionResponseDto> FilterBy(ReactionQueryParameters filterParameters);
    }

}
