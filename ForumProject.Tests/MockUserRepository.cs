using ForumProject.Models;
using ForumProject.Repositories.Contracts;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumProject.Tests
{
    public class MockUsersRepository
    {
        public Mock<IUserRepository> GetMockRepository()
        {
            var mockRepository = new Mock<IUserRepository>();

            var sampleUsers = new List<User>
            {
                TestHelper.GetTestUser()
            };

            mockRepository.Setup(x => x.GetAllUsers()).Returns(sampleUsers);
            mockRepository.Setup(x => x.GetUserById(It.IsAny<int>())).Returns((int id) => sampleUsers.FirstOrDefault(user => user.Id == id));
            mockRepository.Setup(x => x.GetUserByUsername(It.IsAny<string>())).Returns((string username) => sampleUsers.FirstOrDefault(user => user.Username == username));

            return mockRepository;
        }
    }
}
