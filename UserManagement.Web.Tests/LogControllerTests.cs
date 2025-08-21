using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Data.Entities;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Controllers;

namespace UserManagement.Web.Tests;

public class LogControllerTests
{
    private readonly Mock<ILogService> _logService = new();


        private LogController CreateController() =>
            new LogController(_logService.Object);

        private List<Log> SetupLogs()
        {
            return new List<Log>
            {
                new Log { Id = 1, UserId = 10, Action = "Login", Details = "User logged in", PerformedBy = "System" },
                new Log { Id = 2, UserId = 20, Action = "PasswordChange", Details = "Password updated", PerformedBy = "Admin" },
                new Log { Id = 3, UserId = 10, Action = "Logout", Details = "User logged out", PerformedBy = "System" }
            };
        }

        [Fact]
        public async Task LogView_WhenIdExists_ReturnsCorrectLog()
        {
            // Arrange
            var controller = CreateController();
            var logs = SetupLogs();
            _logService.Setup(s => s.GetLogsAsync()).ReturnsAsync(logs);

            // Act
            var result = await controller.LogView(2);

            // Assert
            result.Model.Should().BeOfType<Log>()
                .Which.Id.Should().Be(2);
        }

        [Fact]
        public async Task ViewAllLogs_ReturnsAllLogs()
        {
            // Arrange
            var controller = CreateController();
            var logs = SetupLogs();
            _logService.Setup(s => s.GetLogsAsync()).ReturnsAsync(logs);

            // Act
            var result = await controller.ViewAllLogs(0);

            // Assert
            result.ViewName.Should().Be("ViewLogs"); // custom view name
            result.Model.Should().BeOfType<List<Log>>()
                .Which.Should().HaveCount(3);
        }

        [Fact]
        public async Task ViewLogs_WhenUserIdProvided_ReturnsUserSpecificLogs()
        {
            // Arrange
            var controller = CreateController();
            var logs = SetupLogs();
            _logService.Setup(s => s.GetLogsAsync()).ReturnsAsync(logs);

            // Act
            var result = await controller.ViewLogs(10);

            // Assert
            var model = result.Model.Should().BeAssignableTo<IEnumerable<Log>>().Subject;
            model.Should().OnlyContain(l => l.UserId == 10);
            model.Should().HaveCount(2);
        }

}
