using ForumProject.Data;
using ForumProject.Models;
using ForumProject.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Collections.Specialized.BitVector32;

public static class InitializeData
{
    public static void Initialize(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.Users.Any())
        {
            return;
        }

        var firstNames = new[] { "Maria", "Elena", "Nina", "Katerina", "Ivanka", "Stefka", "Galina", "Margarita", "Rada", "Luba" };
        var lastNames = new[] { "Ivanova", "Petrova", "Stoimenova", "Georgieva", "Dimitrova", "Popova", "Nikolova", "Hristova", "Todorova", "Borisova" };
        var titles = new[]
        {
            "My Favorite Knitting Patterns",
            "A Family Recipe for Banitsa",
            "Childhood Stories from the Village",
            "How to Make the Perfect Moussaka",
            "Knitting Tips for Beginners",
            "My Experience with Home Remedies",
            "The Joy of Gardening",
            "Traditional Bulgarian Songs",
            "Making Your Own Yogurt at Home",
            "A Trip Down Memory Lane"
        };
        var postContents = new[]
                {
                    "I wanted to share my favorite knitting patterns that I've been using for years. They are simple yet elegant. Each pattern holds a special place in my heart as they were the first ones I mastered when I started knitting. I've included detailed step-by-step instructions, tips for choosing the right yarn, and variations to make each project unique. Whether you're knitting a cozy scarf, a warm blanket, or a delicate lace shawl, these patterns are sure to inspire and bring joy to your knitting journey.",

                    "This is a family recipe for Banitsa that has been passed down for generations. It's always a hit at family gatherings. Banitsa, a traditional Bulgarian pastry, is made with layers of thin dough filled with a mixture of eggs, cheese, and yogurt. The crispy, golden crust combined with the savory filling makes it irresistible. In this post, I'll walk you through the process of making Banitsa from scratch, sharing tips and tricks to ensure it turns out perfectly every time. From rolling the dough to layering the filling, you'll learn the secrets behind this beloved family dish.",

                    "Here are some stories from my childhood in the village. Life was simple and full of joy. Growing up, we didn't have the modern conveniences of today, but we had a strong sense of community and an appreciation for the little things in life. I fondly remember the summers spent playing in the fields, the smell of freshly baked bread from the village bakery, and the warmth of the sun on my face as I helped my grandparents in the garden. These stories capture the essence of a bygone era, where life was slower and happiness was found in the everyday moments.",

                    "Let me share with you my secrets to making the perfect moussaka. It's a family favorite. Moussaka, a delicious Mediterranean dish, is a layered casserole made with eggplant, potatoes, ground meat, and a creamy béchamel sauce. Over the years, I've perfected my recipe, ensuring each layer is flavorful and the textures are just right. In this post, I'll guide you through the steps of preparing moussaka, from selecting the best ingredients to mastering the art of layering. With my tips, you'll be able to create a moussaka that's rich, hearty, and bursting with flavor.",

                    "For those new to knitting, these tips will help you get started and avoid common mistakes. Knitting can seem daunting at first, but with the right guidance, anyone can learn to knit beautiful items. I'll cover the basics, such as choosing the right needles and yarn, understanding knitting terminology, and mastering essential stitches. Additionally, I'll share solutions to common problems beginners face, like fixing dropped stitches and maintaining even tension. With these tips, you'll build a strong foundation in knitting and gain the confidence to tackle more advanced projects.",

                    "Over the years, I've learned many home remedies from my grandmother. Here are some that really work. My grandmother was a wealth of knowledge when it came to natural healing. She believed in the power of herbs, essential oils, and simple kitchen ingredients to treat common ailments. In this post, I'll share remedies for colds, headaches, digestive issues, and more. From soothing herbal teas to effective poultices, these tried-and-true remedies have stood the test of time and continue to provide relief without the need for pharmaceuticals.",

                    "Gardening has been a passion of mine for years. Here are some tips for growing your own vegetables. There's nothing quite like the satisfaction of harvesting your own fresh produce. Whether you have a large garden or a small balcony, these tips will help you cultivate a thriving vegetable garden. I'll cover everything from soil preparation and planting techniques to pest control and watering schedules. Plus, I'll share my favorite vegetable varieties that are easy to grow and yield bountiful harvests. With these tips, you'll enjoy a successful and rewarding gardening experience.",

                    "These traditional Bulgarian songs bring back so many memories. I hope you enjoy them as much as I do. Music has always been an integral part of Bulgarian culture, and these songs hold a special place in my heart. They remind me of family gatherings, festive celebrations, and quiet evenings by the fire. In this post, I'll introduce you to some of my favorite traditional Bulgarian songs, along with their histories and meanings. Whether you're familiar with Bulgarian music or discovering it for the first time, these songs will transport you to a place of nostalgia and cultural richness.",

                    "Making your own yogurt at home is easier than you think. Here's how I do it. Homemade yogurt is not only delicious but also packed with probiotics that are great for your health. With just a few ingredients and some patience, you can create creamy, tangy yogurt right in your kitchen. I'll guide you through the process, from choosing the right milk and starter culture to incubating and straining the yogurt. You'll also learn how to customize your yogurt with different flavors and add-ins. Once you try homemade yogurt, you'll never want to go back to store-bought.",

                    "Join me as I take a trip down memory lane and share some of my favorite memories. Life is a collection of moments, and I've been fortunate to have many memorable ones. In this post, I'll share stories from different stages of my life, from childhood adventures and teenage escapades to significant milestones and cherished family moments. Each story is a glimpse into my past, offering insights into the experiences and people who have shaped who I am today. I hope these memories resonate with you and inspire you to reflect on your own journey."
                };

        var commentContents = new[]
        {
            "Thank you for sharing this. It's so helpful!",
            "This brings back so many memories.",
            "I can't wait to try this recipe.",
            "These tips are amazing, thank you!",
            "Such a lovely story, thank you for sharing.",
            "I've been looking for this information for so long.",
            "This is exactly what I needed, thank you!",
            "What a wonderful post, thank you!",
            "I love this! So many great ideas.",
            "Thank you for sharing your knowledge with us."
        };

        var users = new List<User>();
        var posts = new List<Post>();
        var comments = new List<Comment>();
        var replies = new List<Comment>();
        var reactionTargets = new List<ReactionTarget>();
        var reactions = new List<Reaction>();

        Random random = new Random();


        var adminUser = new User
        {
            Username = "admin",
            Password = "admin",
            FirstName = "Admin",
            LastName = "User",
            Email = "admin@example.com",
            IsAdmin = true,
            IsBlocked = false,
        };
        users.Add(adminUser);

        for (int i = 1; i <= 10; i++)
        {
            var firstName = firstNames[random.Next(firstNames.Length)];
            var lastName = lastNames[random.Next(lastNames.Length)];
            var username = $"{firstName}_{lastName}";

            var user = new User
            {
                Username = username,
                Password = $"123",
                FirstName = firstName,
                LastName = lastName,
                Email = $"{username}@example.com",
                IsAdmin = false,
                IsBlocked = false,
                ProfilePictureUrl = $"/images/gm/1 ({i}).webp"
            };
            users.Add(user);
        }

        context.Users.AddRange(users);
        context.SaveChanges();

        for (int i = 1, j = 0; i <= 10; i++)
        {
            var user = users[i];
            var postTitle = titles[i - 1];
            var postContent = postContents[random.Next(postContents.Length)];

            var post = new Post
            {
                Title = postTitle,
                Content = postContent,
                UserId = user.Id,
                Category = (PostCategory)random.Next(0, Enum.GetValues(typeof(PostCategory)).Length)
            };
            if (i % 2 == 0)
            {
                j++;
                post.ImageUrl = $"/images/pimg/1 ({j}).webp";
            }

            posts.Add(post);
        }

        context.Posts.AddRange(posts);
        context.SaveChanges();

        for (int i = 0; i < 20; i++)
        {
            var post = posts[random.Next(0, posts.Count)];
            var user = users[random.Next(0, users.Count)];
            var commentContent = commentContents[random.Next(commentContents.Length)];

            var comment = new Comment
            {
                Content = commentContent,
                PostId = post.Id,
                UserId = user.Id,
                CreatedDate = DateTime.UtcNow,
                ParentCommentId = null
            };
            comments.Add(comment);
        }

        context.Comments.AddRange(comments);
        context.SaveChanges();

        for (int i = 0; i < comments.Count; i++)
        {
            var comment = comments[i];
            var user = users[(i + 1) % users.Count];
            var replyContent = commentContents[random.Next(commentContents.Length)];

            var reply = new Comment
            {
                Content = replyContent,
                PostId = comment.PostId,
                UserId = user.Id,
                CreatedDate = DateTime.UtcNow,
                ParentCommentId = comment.Id
            };
            replies.Add(reply);
        }

        context.Comments.AddRange(replies);
        context.SaveChanges();

        foreach (var post in posts)
        {
            var postReactionTarget = new ReactionTarget
            {
                PostId = post.Id,
                CreatedDate = DateTime.UtcNow
            };
            reactionTargets.Add(postReactionTarget);
        }

        foreach (var comment in comments)
        {
            var commentReactionTarget = new ReactionTarget
            {
                CommentId = comment.Id,
                CreatedDate = DateTime.UtcNow
            };
            reactionTargets.Add(commentReactionTarget);
        }

        context.ReactionTargets.AddRange(reactionTargets);
        context.SaveChanges();
        //var reactionTypes = Enum.GetValues(typeof(ReactionType));
        foreach (var reactionTarget in reactionTargets)
        {
            var reactionCount = random.Next(1, 5);
            for (int i = 0; i < reactionCount; i++)
            {

                var user = users[random.Next(users.Count)];
                var randomCount = random.Next(0, Enum.GetNames(typeof(ReactionType)).Length);
                var reactionT = (ReactionType)randomCount;
                var reaction = new Reaction
                {
                    CreatedDate = DateTime.UtcNow,
                    ReactionType = reactionT,
                    UserId = user.Id,
                    ReactionTargetId = reactionTarget.Id
                };
                reactions.Add(reaction);
            }

        }

        context.Reactions.AddRange(reactions);
        context.SaveChanges();
    }
}
