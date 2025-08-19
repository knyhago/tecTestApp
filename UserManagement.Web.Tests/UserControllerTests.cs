using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Data.Tests
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userService = new();
        private readonly Mock<ILogger<User>> _mockLogger = new();
        private readonly Mock<ILogService> _mockLogService = new();

        private UsersController CreateController() =>
            new(_userService.Object, _mockLogger.Object, _mockLogService.Object);

        private User[] SetupUsers()
        {
            var users = new[]
            {
                new User { Forename = "Johnny", Surname = "User", Email = "juser@example.com", IsActive = true }
            };

            // IUserService.GetAllAsync() returns Task<IEnumerable<User>>
            _userService.Setup(s => s.GetAllAsync())
                    .ReturnsAsync(users.ToList());

            return users;
        }

        [Fact]
        public async Task List_WhenServiceReturnsUsers_ModelMustContainUsers()
        {
            // Arrange
            var controller = CreateController();
            var users = SetupUsers();

            // Act
            var result = await controller.List(); // await because it's async

            // Assert
            result.Model
                .Should().BeOfType<UserListViewModel>()
                .Which.Items.Should().BeEquivalentTo(users);
        }
    }
}
