using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForumProject.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }
        [ForeignKey("Post")]
        public int PostId { get; set; }
        public Post Post { get; set; }
        [ForeignKey("ParentComment")]
        public int? ParentCommentId { get; set; }
        public Comment? ParentComment { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedAt { get; set; }

        public ICollection<ReactionTarget> ReactionTargets { get; set; } = new List<ReactionTarget>();
        public ICollection<Comment> Replies { get; set; } = new List<Comment>();
    }
}
