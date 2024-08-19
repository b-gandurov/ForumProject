    using System.ComponentModel.DataAnnotations;

    namespace ForumProject.Models
    {
        public class ReactionTarget
        {
            [Key]
            public int Id { get; set; }
            public int? PostId { get; set; }
            public int? CommentId { get; set; }
            public DateTime CreatedDate { get; set; }

            public Post? Post { get; set; }
            public Comment? Comment { get; set; }

            public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
        }
    }
