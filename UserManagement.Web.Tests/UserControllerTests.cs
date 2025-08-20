using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UserManagement.Contracts.DTOS;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

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

        private List<User> SetupUsers()
        {
            return new List<User>
            {
                new User
                {
                    Id = 1,
                    Forename = "Johnny",
                    Surname = "User",
                    Email = "juser@example.com",
                    IsActive = true,
                    DateOfBirth = new System.DateOnly(1990, 1, 1)
                }
            };
        }


       [Fact]
        public async Task List_WhenServiceReturnsUsers_ModelMustContainUsers()
        {
            // Arrange
            var controller = CreateController();
            var users = SetupUsers(); // List<User>

            // Setup mock to return users
            _userService.Setup(s => s.GetAllAsync())
                        .ReturnsAsync(users);

            // Setup mock ToDtoList if needed
            var expectedDtos = users.Select(u => new UserDto(
                u.Id,
                u.Forename,
                u.Surname,
                u.DateOfBirth,
                u.Email,
                u.IsActive
            )).ToList();

            _userService.Setup(s => s.ToDtoList(It.IsAny<List<User>>()))
                        .Returns((List<User> u) =>
                            u.Select(x => new UserDto(
                                x.Id,
                                x.Forename,
                                x.Surname,
                                x.DateOfBirth,
                                x.Email,
                                x.IsActive
                            )).ToList());

            // Act
            var result = await controller.List();

            // Assert
            result.Model
                .Should().BeOfType<List<UserDto>>()
                .Which.Should().BeEquivalentTo(expectedDtos);
        }
    }
}
