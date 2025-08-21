using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;

namespace UserManagement.Data.Tests
{
    public class DataContextTests
    {
       [Fact]
        public async Task GetAll_ShouldReturnAllEntities()
        {
            // Arrange
            var context = CreateContext();
            var newUser = new User
            {
                Forename = "Alice",
                Surname = "Wonder",
                Email = "alice@example.com"
            };
            await context.Create(newUser);

            // Act
            var users = await context.GetAll<User>();

            // Assert
            users.Should().ContainSingle(u => u.Email == "alice@example.com");
        }

        [Fact]
        public async Task GetById_WhenEntityExists_ShouldReturnEntity()
        {
            // Arrange
            var context = CreateContext();
            var newUser = new User
            {
                Forename = "Bob",
                Surname = "Builder",
                Email = "bob@example.com"
            };
            await context.Create(newUser);

            // Act
            var user = await context.GetById<User>(newUser.Id);

            // Assert
            user.Should().NotBeNull();
            user!.Email.Should().Be("bob@example.com");
        }

        [Fact]
        public async Task GetById_WhenEntityDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var context = CreateContext();

            // Act
            var user = await context.GetById<User>(999);

            // Assert
            user.Should().BeNull();
        }

        [Fact]
        public async Task Create_ShouldAddEntity()
        {
            // Arrange
            var context = CreateContext();
            var newUser = new User
            {
                Forename = "Charlie",
                Surname = "Chaplin",
                Email = "charlie@example.com"
            };

            // Act
            await context.Create(newUser);
            var users = await context.GetAll<User>();

            // Assert
            users.Should().ContainSingle(u => u.Email == "charlie@example.com");
        }

        [Fact]
        public async Task Update_ShouldModifyEntity()
        {
            // Arrange
            var context = CreateContext();
            var newUser = new User
            {
                Forename = "David",
                Surname = "Goliath",
                Email = "david@example.com"
            };
            await context.Create(newUser);

            // Act
            newUser.Surname = "Updated";
            await context.Update(newUser);

            var updatedUser = await context.GetById<User>(newUser.Id);

            // Assert
            updatedUser!.Surname.Should().Be("Updated");
        }

        [Fact]
        public async Task Delete_ShouldRemoveEntity()
        {
            // Arrange
            var context = CreateContext();
            var newUser = new User
            {
                Forename = "Eve",
                Surname = "Online",
                Email = "eve@example.com"
            };
            await context.Create(newUser);

            // Act
            await context.Delete(newUser);
            var users = await context.GetAll<User>();

            // Assert
            users.Should().NotContain(u => u.Email == "eve@example.com");
        }

        private static DataContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new DataContext(options);

            // Seed initial data (optional)
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
