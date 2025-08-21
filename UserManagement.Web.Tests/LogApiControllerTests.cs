using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Data.Entities;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Controllers;

namespace UserManagement.Web.Tests;

public class LogApiControllerTests
{
    private readonly Mock<ILogService> _logService = new();

        private LogApiController CreateController() =>
            new(_logService.Object);

        private List<Log> SetupLogs()
        {
            return new List<Log>
            {
                new Log { Id = 1, UserId = 10, Action = "Login", Details = "User logged in", PerformedBy = "System" },
                new Log { Id = 2, UserId = 20, Action = "Update", Details = "Profile updated", PerformedBy = "Admin" },
                new Log { Id = 3, UserId = 10, Action = "Logout", Details = "User logged out", PerformedBy = "System" }
            };
        }

        [Fact]
        public async Task GetAllLogs_WhenLogsExist_ReturnsAllLogs()
        {
            // Arrange
            var controller = CreateController();
            var logs = SetupLogs();

            _logService.Setup(s => s.GetLogsAsync())
                       .ReturnsAsync(logs);

            // Act
            var result = await controller.GetAllLogs();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedLogs = okResult.Value.Should().BeAssignableTo<IEnumerable<Log>>().Subject;

            returnedLogs.Should().BeEquivalentTo(logs);
        }

        [Fact]
        public async Task GetLogs_WhenUserIdExists_ReturnsFilteredLogs()
        {
            // Arrange
            var controller = CreateController();
            var logs = SetupLogs();
            var userId = 10;

            _logService.Setup(s => s.GetLogsAsync())
                       .ReturnsAsync(logs);

            // Act
            var result = await controller.GetLogs(userId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedLogs = okResult.Value.Should().BeAssignableTo<IEnumerable<Log>>().Subject;

            returnedLogs.Should().OnlyContain(l => l.UserId == userId);
        }

        [Fact]
        public async Task Create_WhenValidLog_CallsAddLogAndReturnsOk()
        {
            // Arrange
            var controller = CreateController();
            var newLog = new Log { Id = 99, UserId = 50, Action = "Create", Details = "New log entry", PerformedBy = "Tester" };

            _logService.Setup(s => s.AddLogAsync(newLog))
                       .Returns(Task.CompletedTask);

            // Act
            var result = await controller.Create(newLog);

            // Assert
            _logService.Verify(s => s.AddLogAsync(newLog), Times.Once);

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(newLog);
        }

}
