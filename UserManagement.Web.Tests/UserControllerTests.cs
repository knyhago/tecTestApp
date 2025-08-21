using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        private User SetupUser()
        {
            return 
                new User
                {
                    Id = 1,
                    Forename = "Johnny",
                    Surname = "User",
                    Email = "juser@example.com",
                    IsActive = true,
                    DateOfBirth = new System.DateOnly(1990, 1, 1)
                };         
        }

        private UserDto SetupUserDto()
        {
            return 
                new UserDto
                (
                    1,
                    "Johnny",
                    "User",
                    new System.DateOnly(1990, 1, 1),
                    "juser@example.com",
                    true
                );
            
        }

    private List<User> SetupUsersList()
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
                DateOfBirth = new DateOnly(1990, 1, 1)
            },
            new User
            {
                Id = 2,
                Forename = "Jane",
                Surname = "Doe",
                Email = "jane@example.com",
                IsActive = false,
                DateOfBirth = new DateOnly(1992, 5, 10)
            }
        };
    }

    private List<UserDto> SetupUserDtoList()
    {
        return new List<UserDto>
        {
            new UserDto(1, "Johnny", "User", new DateOnly(1990,1,1), "juser@example.com", true),
            new UserDto(2, "Jane", "Doe", new DateOnly(1992,5,10), "jane@example.com", false)
        };
    }



       [Fact]
        public async Task List_WhenServiceReturnsUsers_ModelMustContainUsers()
        {
            // Arrange
            var controller = CreateController();
            var users = SetupUsersList(); // List<User>

            var expectedDtos = SetupUserDtoList();

            // Setup mock to return users
            _userService.Setup(s => s.GetAllAsync())
                        .ReturnsAsync(users);

            _userService.Setup(s=>s.ToDtoList(users)).Returns(expectedDtos);

            // Act
            var result = await controller.List();

            // Assert
            result.Model
                .Should().BeOfType<List<UserDto>>()
                .Which.Should().BeEquivalentTo(expectedDtos);
        }

    [Fact]
    public async Task FilterList_WhenServiceReturnsActiveUsers_ModelContainsOnlyActiveUsers()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsersList(); // contains one active, one inactive
        var expectedDtos = SetupUserDtoList().Where(u => u.IsActive).ToList();

        // Setup service to return only active users
        _userService.Setup(s => s.FilterByActiveAsync(true))
                    .ReturnsAsync(users.Where(u => u.IsActive).ToList());

        // Setup ToDtoList to convert filtered users to DTOs
        _userService.Setup(s => s.ToDtoList(It.Is<List<User>>(list => list.All(u => u.IsActive))))
                    .Returns(expectedDtos);

        // Act
        var result = await controller.FilterList(true);

        // Assert
        result.Model
            .Should().BeOfType<List<UserDto>>()
            .Which.Should().BeEquivalentTo(expectedDtos);
    }

    [Fact]
    public async Task FilterList_WhenServiceReturnsInactiveUsers_ModelContainsOnlyInactiveUsers()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsersList(); // contains one active, one inactive
        var expectedDtos = SetupUserDtoList().Where(u => !u.IsActive).ToList();

        // Setup service to return only inactive users
        _userService.Setup(s => s.FilterByActiveAsync(false))
                    .ReturnsAsync(users.Where(u => !u.IsActive).ToList());

        // Setup ToDtoList to convert filtered users to DTOs
        _userService.Setup(s => s.ToDtoList(It.Is<List<User>>(list => list.All(u => !u.IsActive))))
                    .Returns(expectedDtos);

        // Act
        var result = await controller.FilterList(false);

        // Assert
        result.Model
            .Should().BeOfType<List<UserDto>>()
            .Which.Should().BeEquivalentTo(expectedDtos);
    }

    
    [Fact]
    public async Task AddEditUserForm_WhenIdMatchesUser_ReturnsUserDtoInView()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsersList(); // contains Id 1 and 2
        var targetUser = users.First(); // Id = 1
        var targetDto = SetupUserDtoList().First();

        _userService.Setup(s => s.GetAllAsync()).ReturnsAsync(users);
        _userService.Setup(s => s.ToDto(It.Is<User>(u => u.Id == targetUser.Id)))
                    .Returns(targetDto);

        // Act
        var result = await controller.AddEditUserForm(targetUser.Id);

        // Assert
        result.Model.Should().BeOfType<UserDto>()
                    .Which.Should().BeEquivalentTo(targetDto);
    }

    [Fact]
    public async Task AddEditUserForm_WhenIdDoesNotMatchUser_ThrowsException()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsersList(); // Ids 1 and 2
        int invalidId = 999;

        _userService.Setup(s => s.GetAllAsync()).ReturnsAsync(users);

        // Act & Assert
        await FluentActions.Invoking(() => controller.AddEditUserForm(invalidId))
            .Should().ThrowAsync<Exception>()
            .WithMessage("User Page can be found to edit");
    }

    // [Fact]
    // public async Task AddEditUserForm_WhenIdIsNullOrZero_ReturnsEmptyView()
    // {
    //     var controller = CreateController();

    // // Act
    //     var resultNull = await controller.AddEditUserForm(null);
    //     var resultZero = await controller.AddEditUserForm(0L);

    //     // Assert
    //     resultZero.Model.Should().BeOfType<UserDto>();

    //     resultZero.Model.As<UserDto>().Id.Should().Be(0);
    // }

    [Fact]
public async Task SubmitForm_WhenIdIsZero_CallsAddAsync()
{
    // Arrange
    var controller = CreateController();
    var userDto = new UserDto(
        0, 
        "John", 
        "Doe", 
        new DateOnly(1990, 1, 1), 
        "jdoe@example.com", 
        true
    );   
     var user = new User { Id = 0, Forename = "John", Surname = "Doe" };

    _userService.Setup(s => s.ToEntity(userDto)).Returns(user);
    _userService.Setup(s => s.AddAsync(user)).Returns(Task.CompletedTask);

    // Act
    var result = await controller.SubmitForm(userDto);

    // Assert
    _userService.Verify(s => s.AddAsync(user), Times.Once);
    _userService.Verify(s => s.UpdateUserAsync(It.IsAny<User>()), Times.Never);

    result.Should().BeOfType<RedirectToActionResult>()
          .Which.ActionName.Should().Be("List");
}

[Fact]
public async Task SubmitForm_WhenIdIsNonZero_CallsUpdateUserAsync()
{
    // Arrange
    var controller = CreateController();
    
    var userDto = new UserDto(
    0, 
    "John", 
    "Doe", 
    new DateOnly(1990, 1, 1), 
    "jdoe@example.com", 
    true
);
    var user = new User { Id = 5, Forename = "John", Surname = "Doe" };

    _userService.Setup(s => s.ToEntity(userDto)).Returns(user);
    _userService.Setup(s => s.UpdateUserAsync(user)).Returns(Task.CompletedTask);

    // Act
    var result = await controller.SubmitForm(userDto);

    // Assert
    _userService.Verify(s => s.UpdateUserAsync(user), Times.Once);
    _userService.Verify(s => s.AddAsync(It.IsAny<User>()), Times.Never);

    result.Should().BeOfType<RedirectToActionResult>()
          .Which.ActionName.Should().Be("List");
}

[Fact]
public async Task DeleteUser_WhenCalled_DeletesCorrectUserAndRedirects()
{
    // Arrange
    var controller = CreateController();
    var users = new List<User>
    {
        new User { Id = 1, Forename = "John", Surname = "Doe" },
        new User { Id = 2, Forename = "Jane", Surname = "Smith" }
    };

    _userService.Setup(s => s.GetAllAsync()).ReturnsAsync(users);
    _userService.Setup(s => s.DeleteUserAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

    // Act
    var result = await controller.DeleteUser(2);

    // Assert
    _userService.Verify(s => s.DeleteUserAsync(It.Is<User>(u => u.Id == 2)), Times.Once);
    
    var redirectResult = result as RedirectToActionResult;
    redirectResult.Should().NotBeNull();
    redirectResult.ActionName.Should().Be("List");
}





    }
}
