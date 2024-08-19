using ForumProject.Models.Enums;
using System;

namespace ForumProject.Models.QueryParameters
{
    public class PostQueryParameters
    {
        public string Title { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }
        public string UserName { get; set; }
        public string SortBy { get; set; } = "CreatedAt";
        public string SortOrder { get; set; } = "desc";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 2;
        public PostCategory? Category { get; set; }
        
    }
}
