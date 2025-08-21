using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Data;
using UserManagement.Data.Entities;
using UserManagement.Services.Domain.Implementations;

namespace UserManagement.Services.Tests;

public class LogServiceTests
{
    private readonly Mock<IDataContext> _mockDataContext = new();
        
        private LogService CreateService() => new(_mockDataContext.Object);

        private Log SampleLog(int id = 1, int userId = 10, string action = "TestAction") =>
            new Log
            {
                Id = id,
                UserId = userId,
                Action = action,
                Details = "Test details",
                PerformedBy = "Tester"
            };

        [Fact]
        public async Task AddLogAsync_WhenCalled_CreatesLog()
        {
            // Arrange
            var service = CreateService();
            var log = SampleLog();

            _mockDataContext.Setup(d => d.Create(log))
                            .Returns(Task.CompletedTask)
                            .Verifiable();

            // Act
            await service.AddLogAsync(log);

            // Assert
            _mockDataContext.Verify(d => d.Create(log), Times.Once);
        }

        [Fact]
        public async Task GetLogsAsync_WhenLogsExist_ReturnsAllLogs()
        {
            // Arrange
            var service = CreateService();
            var logs = new List<Log>
            {
                SampleLog(1),
                SampleLog(2, 20, "AnotherAction")
            };

            _mockDataContext.Setup(d => d.GetAll<Log>())
                            .ReturnsAsync(logs);

            // Act
            var result = await service.GetLogsAsync();

            // Assert
            result.Should().BeEquivalentTo(logs);
        }

        [Fact]
        public async Task GetLogsAsync_WhenNoLogsExist_ReturnsEmptyList()
        {
            // Arrange
            var service = CreateService();
            var logs = new List<Log>();

            _mockDataContext.Setup(d => d.GetAll<Log>())
                            .ReturnsAsync(logs);

            // Act
            var result = await service.GetLogsAsync();

            // Assert
            result.Should().BeEmpty();
        }

}
