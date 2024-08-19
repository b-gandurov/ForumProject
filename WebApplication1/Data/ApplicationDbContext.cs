using Microsoft.EntityFrameworkCore;
using ForumProject.Models;
using ForumProject.Models.Enums;
using System;
using System.Linq;

namespace ForumProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<ReactionTarget> ReactionTargets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.ImageUrl).HasMaxLength(500);

                entity.HasOne(p => p.User)
                    .WithMany(u => u.Posts)
                    .HasForeignKey(p => p.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(c => c.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(c => c.PostId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.User)
                    .WithMany(u => u.Comments)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.ParentComment)
                    .WithMany(c => c.Replies)
                    .HasForeignKey(c => c.ParentCommentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ReactionTarget>(entity =>
            {
                entity.HasKey(rt => rt.Id);
                entity.Property(rt => rt.Id).ValueGeneratedOnAdd();

                entity.HasOne(rt => rt.Post)
                    .WithMany(p => p.ReactionTargets)
                    .HasForeignKey(rt => rt.PostId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(rt => rt.Comment)
                    .WithMany(c => c.ReactionTargets)
                    .HasForeignKey(rt => rt.CommentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Reaction>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Id).ValueGeneratedOnAdd();

                entity.HasOne(r => r.ReactionTarget)
                    .WithMany(rt => rt.Reactions)
                    .HasForeignKey(r => r.ReactionTargetId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.User)
                    .WithMany(u => u.Reactions)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Post>().HasQueryFilter(p => p.DeletedAt == null).HasQueryFilter(p => p.User.DeletedAt == null);
            modelBuilder.Entity<Comment>().HasQueryFilter(c => !c.Post.DeletedAt.HasValue);
            modelBuilder.Entity<Reaction>().HasQueryFilter(r => !r.User.DeletedAt.HasValue && !r.DeletedAt.HasValue);
            modelBuilder.Entity<ReactionTarget>().HasQueryFilter(rt =>
                (!rt.PostId.HasValue || !rt.Post.DeletedAt.HasValue) &&
                (!rt.CommentId.HasValue || !rt.Comment.DeletedAt.HasValue)
            );
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted))
            {
                switch (entry.Entity)
                {
                    case User user:
                        MarkUserAsDeleted(user);
                        entry.State = EntityState.Modified;
                        break;

                    case Post post:
                        entry.State = EntityState.Modified;
                        post.DeletedAt = DateTime.UtcNow;
                        break;

                    case Comment comment:
                        entry.State = EntityState.Modified;
                        comment.DeletedAt = DateTime.UtcNow;
                        break;

                    case Reaction reaction:
                        entry.State = EntityState.Modified;
                        reaction.DeletedAt = DateTime.UtcNow;
                        break;

                    case ReactionTarget reactionTarget:
                        entry.State = EntityState.Modified;
                        if (reactionTarget.Post != null)
                            reactionTarget.Post.DeletedAt = DateTime.UtcNow;
                        if (reactionTarget.Comment != null)
                            reactionTarget.Comment.DeletedAt = DateTime.UtcNow;
                        break;
                }
            }
            return base.SaveChanges();
        }

        internal void MarkUserAsDeleted(User user)
        {
            var posts = Posts.Where(p => p.UserId == user.Id).ToList();
            var comments = Comments.Where(c => c.UserId == user.Id).ToList();
            var reactions = Reactions.Where(r => r.UserId == user.Id).ToList();
            var reactionTargets = ReactionTargets.Where(rt => rt.Post.UserId == user.Id || rt.Comment.UserId == user.Id).ToList();

            foreach (var post in posts)
            {
                post.DeletedAt = DateTime.UtcNow;
            }

            foreach (var comment in comments)
            {
                comment.DeletedAt = DateTime.UtcNow;
            }

            foreach (var reaction in reactions)
            {
                reaction.DeletedAt = DateTime.UtcNow;
            }

            foreach (var reactionTarget in reactionTargets)
            {
                if (reactionTarget.Post != null)
                {
                    reactionTarget.Post.DeletedAt = DateTime.UtcNow;
                }

                if (reactionTarget.Comment != null)
                {
                    reactionTarget.Comment.DeletedAt = DateTime.UtcNow;
                }
            }

            user.DeletedAt = DateTime.UtcNow;
        }
    }
}
