using ForumProject.Models.Enums;

namespace ForumProject.Models.QueryParameters
{
    public class ReactionQueryParameters
    {

        //TODO Maria

        public int PostId { get; set; }// filters reactions by ID of the post they belong to

        public int CommentId { get; set; }// filters reactions by ID of the post they belong to

        public int UserId { get; set; }// filters reactions by ID of the User who created them

        public ReactionType Type { get; set; } // filters reactions by type (Like, Dislike,etc..)
    }
}
