namespace ForumProject.Models.QueryParameters
{
    public class CommentQueryParameters
    {
        public int? PostId { get; set; } 
        public int? UserId { get; set; } 
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; } 
    }
}
