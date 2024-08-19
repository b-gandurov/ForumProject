using ForumProject.Models;
using ForumProject.Models.Enums;
using ForumProject.Models.QueryParameters;
using ForumProject.Repositories.Contracts;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace ForumProject.Tests.Mocks
{
    public class MockPostsRepository
    {
        public Mock<IPostRepository> GetMockRepository()
        {
            var mockRepository = new Mock<IPostRepository>();

            var samplePosts = new List<Post>
            {
                TestHelper.GetTestPost()
            };

            mockRepository.Setup(x => x.GetAllPosts()).Returns(samplePosts.AsQueryable());
            mockRepository.Setup(x => x.GetPostById(It.IsAny<int>())).Returns((int id) => samplePosts.FirstOrDefault(post => post.Id == id));
            mockRepository.Setup(x => x.AddPost(It.IsAny<Post>())).Callback((Post post) => samplePosts.Add(post));
            mockRepository.Setup(x => x.UpdatePost(It.IsAny<Post>())).Callback((Post post) => {
                var existingPost = samplePosts.FirstOrDefault(p => p.Id == post.Id);
                if (existingPost != null)
                {
                    existingPost.Title = post.Title;
                    existingPost.Content = post.Content;
                    existingPost.Category = post.Category;
                }
            });
            mockRepository.Setup(x => x.DeletePost(It.IsAny<int>())).Callback((int id) => {
                var post = samplePosts.FirstOrDefault(p => p.Id == id);
                if (post != null)
                {
                    samplePosts.Remove(post);
                }
            });

            return mockRepository;
        }
    }
}
