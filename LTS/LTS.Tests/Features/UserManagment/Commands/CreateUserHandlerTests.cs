using LTS.API.Domain.Entities;
using LTS.API.Domain.Enums;
using LTS.API.Features.UserManangement.Commands.Authentication.CreateUser;
using LTS.API.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using FluentAssertions;

namespace LTS.Tests.Features.UserManagement.Commands;

public class CreateUserHandlerTests
{
    private AppDbContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    private Mock<IPasswordHasher<User>> GetPasswordHasher()
    {
        var mock = new Mock<IPasswordHasher<User>>();
        mock.Setup(x => x.HashPassword(It.IsAny<User>(), It.IsAny<string>()))
            .Returns("hashed_password");
        return mock;
    }

    [Fact]
    public async Task Handle_ShouldCreateUserAndOrganization_WhenEmailIsUnique()
    {
        // Arrange
        var context = GetInMemoryContext();
        var handler = new CreateUserCommandHandler(context, GetPasswordHasher().Object);

        var command = new CreateUserCommand(
            OrganizationName: "Test Org",
            SubscriptionPlan: SubscriptionPlan.Free,
            OwnerName: "Test User",
            Email: "test@test.com",
            Password: "Test@123"
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be("Registration successful");

        var userInDb = await context.Users.FirstOrDefaultAsync(u => u.Email == "test@test.com");
        userInDb.Should().NotBeNull();
        userInDb!.Name.Should().Be("Test User");
        userInDb.PasswordHash.Should().Be("hashed_password");

        var orgInDb = await context.Organizations.FirstOrDefaultAsync();
        orgInDb.Should().NotBeNull();
        orgInDb!.OrganizationName.Should().Be("Test Org");
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenEmailAlreadyExists()
    {
        // Arrange
        var context = GetInMemoryContext();

        context.Users.Add(new User
        {
            Id = Guid.NewGuid(),
            Email = "test@test.com",
            Name = "Existing User",
            PasswordHash = "hash",
            IsActive = false,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "test@test.com"
        });
        await context.SaveChangesAsync();

        var handler = new CreateUserCommandHandler(context, GetPasswordHasher().Object);

        var command = new CreateUserCommand(
            OrganizationName: "Another Org",
            SubscriptionPlan: SubscriptionPlan.Free,
            OwnerName: "Another User",
            Email: "test@test.com",  // duplicate email
            Password: "Test@123"
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Email already exists"); // handler se match karta hai

        // Verify no new user or org was added
        var userCount = await context.Users.CountAsync();
        userCount.Should().Be(1);

        var orgCount = await context.Organizations.CountAsync();
        orgCount.Should().Be(0);
    }

    [Fact]
    public async Task Handle_ShouldSetCorrectMaxUsers_BasedOnPlan()
    {
        // Arrange
        var context = GetInMemoryContext();
        var handler = new CreateUserCommandHandler(context, GetPasswordHasher().Object);

        var command = new CreateUserCommand(
            OrganizationName: "Pro Org",
            SubscriptionPlan: SubscriptionPlan.Pro,
            OwnerName: "Pro User",
            Email: "pro@test.com",
            Password: "Test@123"
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var org = await context.Organizations.FirstOrDefaultAsync();
        org.Should().NotBeNull();
        org!.MaxUsers.Should().Be(20);
    }

    [Fact]
    public async Task Handle_ShouldLinkUserToOrganization_WhenCreated()
    {
        // Arrange
        var context = GetInMemoryContext();
        var handler = new CreateUserCommandHandler(context, GetPasswordHasher().Object);

        var command = new CreateUserCommand(
            OrganizationName: "Linked Org",
            SubscriptionPlan: SubscriptionPlan.Free,
            OwnerName: "Linked User",
            Email: "linked@test.com",
            Password: "Test@123"
        );

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert — user ka OrganizationId, org ke Id se match karna chahiye
        var org = await context.Organizations.FirstOrDefaultAsync();
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == "linked@test.com");

        org.Should().NotBeNull();
        user.Should().NotBeNull();
        user!.OrganizationId.Should().Be(org!.Id);
    }
}