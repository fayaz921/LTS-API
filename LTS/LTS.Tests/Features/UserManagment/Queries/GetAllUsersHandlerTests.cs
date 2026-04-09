using LTS.API.Domain.Entities;
using LTS.API.Domain.Enums;
using LTS.API.Features.UserManangement.Queries.GetAllUsers;
using LTS.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

namespace LTS.Tests.Features.UserManagement.Queries;

public class GetAllUsersHandlerTests
{
    private AppDbContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task Handle_ShouldReturnOnlyActiveUsers()
    {
        // Arrange
        var context = GetInMemoryContext();

        context.Users.AddRange(
            new User
            {
                Id = Guid.NewGuid(),
                Name = "Active User",
                Email = "active@test.com",
                PasswordHash = "hash",
                Role = UserRole.User,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "active@test.com"
            },
            new User
            {
                Id = Guid.NewGuid(),
                Name = "Inactive User",
                Email = "inactive@test.com",
                PasswordHash = "hash",
                Role = UserRole.User,
                IsActive = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "inactive@test.com"
            }
        );
        await context.SaveChangesAsync();

        var handler = new GetAllUsersHandler(context);

        // Act
        var result = await handler.Handle(new GetAllUserQuery(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().Email.Should().Be("active@test.com");
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoActiveUsers()
    {
        // Arrange
        var context = GetInMemoryContext();
        var handler = new GetAllUsersHandler(context);

        // Act
        var result = await handler.Handle(new GetAllUserQuery(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnCorrectFields()
    {
        // Arrange
        var context = GetInMemoryContext();

        context.Users.Add(new User
        {
            Id = Guid.NewGuid(),
            Name = "Fayaz",
            Email = "fayaz@test.com",
            PasswordHash = "hash",
            Role = UserRole.User,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "fayaz@test.com"
        });
        await context.SaveChangesAsync();

        var handler = new GetAllUsersHandler(context);

        // Act
        var result = await handler.Handle(new GetAllUserQuery(), CancellationToken.None);

        // Assert
        var user = result.First();
        user.Name.Should().Be("Fayaz");
        user.Email.Should().Be("fayaz@test.com");
        user.Role.Should().Be("User");
        user.IsActive.Should().BeTrue();
    }
}