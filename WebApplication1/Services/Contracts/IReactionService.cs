using ForumProject.Models.DTOs;

namespace ForumProject.Services.Contracts
{
    public interface IReactionService
    {
        ReactionResponseDto GetReactionById(int id);
        void AddReaction(ReactionRequestDto reactionDto);
        void UpdateReaction(ReactionRequestDto reactionDto);
        void DeleteReaction(int id);
        IEnumerable<ReactionResponseDto> GetReactionsByPostId(int postId);

        IEnumerable<ReactionResponseDto> GetReactionsByCommentId(int commentId);
    }

}
