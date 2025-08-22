using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Contracts.DTOS;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Controllers;

namespace UserManagement.Web.Tests;

public class UsersApiControllerTests
{
    private readonly Mock<IUserService> _userService = new();
        private readonly Mock<ILogService> _logService = new();

        private UsersApiController CreateController() =>
            new UsersApiController(_userService.Object, _logService.Object);

        private User SetupUser(long id = 1) =>
            new User
            {
                Id = id,
                Forename = "John",
                Surname = "Doe",
                DateOfBirth = new DateOnly(1990, 1, 1),
                Email = "john@example.com",
                IsActive = true
            };

        [Fact]
        public async Task GetAll_WhenUsersExist_ReturnsOkWithDtos()
        {
            // Arrange
            var controller = CreateController();
            var users = new List<User> { SetupUser(1), SetupUser(2) };
            _userService.Setup(s => s.GetAllAsync()).ReturnsAsync(users);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var dtos = okResult.Value.Should().BeAssignableTo<List<UserDto>>().Subject;
            dtos.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetById_WhenUserExists_ReturnsOkWithDto()
        {
            // Arrange
            var controller = CreateController();
            var user = SetupUser(5);
            _userService.Setup(s => s.GetUserByIdAsync(5)).ReturnsAsync(user);

            // Act
            var result = await controller.GetById(5);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var dto = okResult.Value.Should().BeOfType<UserDto>().Subject;
            dto.Id.Should().Be(5);
        }

        [Fact]
        public async Task GetById_WhenUserDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var controller = CreateController();
            _userService.Setup(s => s.GetUserByIdAsync(99)).ReturnsAsync((User?)null);

            // Act
            var result = await controller.GetById(99);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Delete_WhenUserExists_DeletesAndReturnsNoContent()
        {
            // Arrange
            var controller = CreateController();
            var user = SetupUser(3);
            _userService.Setup(s => s.GetUserByIdAsync(3)).ReturnsAsync(user);

            // Act
            var result = await controller.Delete(3);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _userService.Verify(s => s.DeleteUserAsync(user), Times.Once);
        }

        [Fact]
        public async Task Delete_WhenUserDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var controller = CreateController();
            _userService.Setup(s => s.GetUserByIdAsync(50)).ReturnsAsync((User?)null);

            // Act
            var result = await controller.Delete(50);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

 [Fact]
      
public async Task Update_WhenUserExists_UpdatesAndReturnsNoContent()
{
    // Arrange
    var controller = CreateController();
    
    var existingUser = SetupUser(10); // user in DB
    var updatedUser = new User
    {
        Id = 10,
        Forename = "Jane",
        Surname = "Smith",
        DateOfBirth = new DateOnly(1985, 5, 20),
        Email = "jane@example.com",
        IsActive = false
    };

    // Mock existing users for duplicate check
    _userService.Setup(s => s.GetAllAsync())
                .ReturnsAsync(new List<User> { existingUser });
    
    // Mock fetching the user to update
    _userService.Setup(s => s.GetUserByIdAsync(10))
                .ReturnsAsync(existingUser);

    // Mock the update method
    _userService.Setup(s => s.UpdateUserAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

    // Act
    var result = await controller.Update(10, updatedUser);

    // Assert
    result.Should().BeOfType<NoContentResult>();
    
    _userService.Verify(s => s.UpdateUserAsync(It.Is<User>(u =>
        u.Id == 10 &&
        u.Forename == "Jane" &&
        u.Surname == "Smith" &&
        u.Email == "jane@example.com" &&
        u.IsActive == false
    )), Times.Once);
}


        [Fact]
        public async Task Update_WhenUserDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var controller = CreateController();
            _userService.Setup(s => s.GetUserByIdAsync(77)).ReturnsAsync((User?)null);

            // Act
            var result = await controller.Update(77, SetupUser(77));

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Create_WhenValidUser_ReturnsCreatedAtAction()
        {
            // Arrange
            var controller = CreateController();
            var newUser = SetupUser(100);

           _userService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<User>());
            _userService.Setup(s => s.AddAsync(newUser)).Returns(Task.CompletedTask);

            // Act
            var result = await controller.Create(newUser);

            // Assert
            var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.RouteValues.Should().NotBeNull();
            createdResult.RouteValues!["id"].Should().Be(newUser.Id);


        }

}
