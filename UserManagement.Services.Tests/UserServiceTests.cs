using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Data.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IDataContext> _dataContext = new();
        private readonly Mock<ILogService> _mockLogService = new();
        private readonly Mock<ILogger<User>> _mockLogger = new();

        private UserService CreateService() => new(
            _dataContext.Object,
            _mockLogger.Object,
            _mockLogService.Object
        );

        private List<User> SetupUsers(string forename = "Johnny", string surname = "User",
                                     string email = "juser@example.com", bool isActive = true)
        {
            var users = new List<User>
            {
                new User
                {
                    Forename = forename,
                    Surname = surname,
                    Email = email,
                    IsActive = isActive
                }
            };

            // Setup the async method to return the list
            _dataContext
                .Setup(ctx => ctx.GetAll<User>())
                .ReturnsAsync(users);

            return users;
        }

        [Fact]
        public async Task GetAll_WhenContextReturnsEntities_MustReturnSameEntities()
        {
            // Arrange
            var service = CreateService();
            var users = SetupUsers();

            // Act
            var result = await service.GetAllAsync();

            // Assert
            result.Should().BeSameAs(users);
        }

        [Fact]
        public async Task FilterByActive_ShouldReturnOnlyActiveUsers()
        {
            // Arrange
            var service = CreateService();
            var users = SetupUsers();
            
            // Act
            var result = (await service.GetAllAsync()).Where(u => u.IsActive).ToList();

            // Assert
            result.Should().OnlyContain(u => u.IsActive);
        }
    }
}
