using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;

namespace UserManagement.Data.Tests
{
    public class DataContextTests
    {
        [Fact]
        public async Task GetAll_WhenNewEntityAdded_MustIncludeNewEntity()
        {
            // Arrange
            var context = CreateContext();

            var entity = new User
            {
                Forename = "Brand New",
                Surname = "User",
                Email = "brandnewuser@example.com"
            };

            await context.Create(entity);

            // Act
            var result = await context.GetAll<User>();

            // Assert
            result.Should().ContainSingle(s => s.Email == entity.Email)
                  .Which.Should().BeEquivalentTo(entity, options => options.Excluding(u => u.Id));
        }

        [Fact]
        public async Task GetAll_WhenDeleted_MustNotIncludeDeletedEntity()
        {
            // Arrange
            var context = CreateContext();

            var entity = new User
            {
                Forename = "Test",
                Surname = "User",
                Email = "testuser@example.com"
            };
            await context.Create(entity);

            // Act
            await context.Delete(entity);
            var result = await context.GetAll<User>();

            // Assert
            result.Should().NotContain(s => s.Email == entity.Email);
        }

        private static DataContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new DataContext(options);

            // Optional: seed initial data
            context.Users.AddRange(new[]
            {
                new User { Id = 1, Forename = "Peter", Surname = "Loew", Email = "ploew@example.com", IsActive = true, DateOfBirth = new DateOnly(1995,6,12) },
                new User { Id = 2, Forename = "Benjamin", Surname = "Gates", Email = "bfgates@example.com", IsActive = true, DateOfBirth = new DateOnly(1995,11,21) }
            });
            context.SaveChanges();

            return context;
        }
    }
}
