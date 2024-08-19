using ForumProject.Models.DTOs;
using ForumProject.Models;
using ForumProject.Models.ViewModels;

namespace ForumProject.Helpers
{
    public interface IModelMapper
    {
        
        User ToUser(UserRequestDto userDTO);
        PostResponseDto ToPostResponseDto(Post post);
        Post ToPost(PostRequestDto postDto);

        Post ToPost(PostViewModel postViewModel);

        PostViewModel ToPostViewModel(Post post);
        CommentResponseDto ToCommentResponseDto(Comment comment);
        Comment ToComment(CommentRequestDto commentDto);

        Comment ToComment(CommentViewModel commentViewModel);
        UserResponseDto ToUserResponseDto(User user);
        UserViewModel ToUserViewModel(User user);

        ReactionResponseDto ToReactionResponseDto(Reaction reaction);
        Reaction ToReaction(ReactionRequestDto reactionDTO);

        ReactionViewModel ToReactionViewModel(Reaction reaction);

        ReplyViewModel ToReplyViewModel(Comment reply);



    }
}
