using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UserManagement.Contracts.DTOS;
using UserManagement.Data.Entities;
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

        private User SampleUser(int id = 1, bool isActive = true) => new()
        {
            Id = id,
            Forename = "Johnny",
            Surname = "User",
            Email = "juser@example.com",
            DateOfBirth = new DateOnly(1990, 1, 1),
            IsActive = isActive
        };

        [Fact]
        public async Task GetById_WhenUserExists_ShouldReturnUser()
        {
            // Arrange
            var user = SampleUser();
            _dataContext.Setup(d => d.GetById<User>(user.Id)).ReturnsAsync(user);
            var service = CreateService();

            // Act
            var result = await service.GetById(user.Id);

            // Assert
            result.Should().Be(user);
        }

        [Fact]
        public async Task GetById_WhenUserDoesNotExist_ShouldThrowException()
        {
            _dataContext.Setup(d => d.GetById<User>(It.IsAny<int>())).ReturnsAsync((User?)null);
            var service = CreateService();

            await Assert.ThrowsAsync<Exception>(() => service.GetById(999));
        }

        [Fact]
        public async Task GetUserByIdAsync_WhenUserExists_ShouldReturnUser()
        {
            var user = SampleUser();
            _dataContext.Setup(d => d.GetAll<User>()).ReturnsAsync(new List<User> { user });
            var service = CreateService();

            var result = await service.GetUserByIdAsync(user.Id);

            result.Should().Be(user);
        }

        [Fact]
        public async Task GetUserByIdAsync_WhenUserDoesNotExist_ShouldThrowException()
        {
            _dataContext.Setup(d => d.GetAll<User>()).ReturnsAsync(new List<User>());
            var service = CreateService();

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetUserByIdAsync(999));
        }

        [Fact]
        public async Task AddAsync_ShouldCreateUserAndLog()
        {
            var user = SampleUser();
            var service = CreateService();

            await service.AddAsync(user);

            _dataContext.Verify(d => d.Create(user), Times.Once);
            _mockLogService.Verify(l => l.AddLogAsync(It.Is<Log>(log => log.Action == "Added")), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_WhenUserExists_ShouldUpdateAndLogChanges()
        {
            var oldUser = SampleUser();
            var updatedUser = SampleUser();
            updatedUser.Surname = "Updated";

            _dataContext.Setup(d => d.GetById<User>(oldUser.Id)).ReturnsAsync(oldUser);
            var service = CreateService();

            await service.UpdateUserAsync(updatedUser);

            _dataContext.Verify(d => d.Update(updatedUser), Times.Once);
            _mockLogService.Verify(l => l.AddLogAsync(It.Is<Log>(log => log.Action == "Edited")), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_WhenUserDoesNotExist_ShouldThrowException()
        {
            _dataContext.Setup(d => d.GetById<User>(It.IsAny<int>())).ReturnsAsync((User?)null);
            var service = CreateService();
            var user = SampleUser();

            await Assert.ThrowsAsync<Exception>(() => service.UpdateUserAsync(user));
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldDeleteAndLog()
        {
            var user = SampleUser();
            var service = CreateService();

            await service.DeleteUserAsync(user);

            _dataContext.Verify(d => d.Delete(user), Times.Once);
            _mockLogService.Verify(l => l.AddLogAsync(It.Is<Log>(log => log.Action == "Deleted")), Times.Once);
        }

        [Fact]
        public void ToDto_ShouldConvertUserToUserDto()
        {
            var user = SampleUser();
            var service = CreateService();

            var dto = service.ToDto(user);

            dto.Should().BeEquivalentTo(new UserDto(user.Id, user.Forename, user.Surname, user.DateOfBirth, user.Email, user.IsActive));
        }

        [Fact]
        public void ToEntity_ShouldConvertDtoToUser()
        {
            var dto = new UserDto(1, "Johnny", "User", new DateOnly(1990, 1, 1), "juser@example.com", true);
            var service = CreateService();

            var entity = service.ToEntity(dto);

            entity.Should().BeEquivalentTo(new User
            {
                Id = dto.Id,
                Forename = dto.Forename,
                Surname = dto.Surname,
                DateOfBirth = dto.DateOfBirth,
                Email = dto.Email,
                IsActive = true
            });
        }

        [Fact]
        public void ToDtoList_ShouldConvertListOfUsersToDtos()
        {
            var users = new List<User> { SampleUser() };
            var service = CreateService();

            var dtos = service.ToDtoList(users);

            dtos.Should().HaveCount(1);
            dtos.First().Id.Should().Be(users[0].Id);
        }
    }
}

