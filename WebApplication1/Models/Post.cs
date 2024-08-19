using ForumProject.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ForumProject.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
        public string? ImageUrl { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public PostCategory Category { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<ReactionTarget> ReactionTargets { get; set; } = new List<ReactionTarget>();
    }
}
