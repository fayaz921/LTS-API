using LTS.API.Domain.Entities;
using LTS.API.Features.UserManangement.Commands.Authentication.LoginUser;
using LTS.API.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using FluentAssertions;

namespace LTS.Tests.Features.UserManagement.Commands;

public class LoginHandlerTests
{
    private AppDbContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenCredentialsAreValid()
    {
        // Arrange
        var context = GetInMemoryContext();
        var passwordHasher = new Mock<IPasswordHasher<User>>();

        passwordHasher
            .Setup(x => x.VerifyHashedPassword(
                It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(PasswordVerificationResult.Success);

        context.Users.Add(new User
        {
            Id = Guid.NewGuid(),
            Email = "fayaz@test.com",
            Name = "Fayaz",
            PasswordHash = "hashed_password",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "fayaz@test.com"
        });
        await context.SaveChangesAsync();

        var handler = new LoginUserCommandHandler(context, passwordHasher.Object);
        var command = new LoginUserCommand("fayaz@test.com", "Test@123");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be("Login Success");
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenUserNotFound()
    {
        // Arrange
        var context = GetInMemoryContext();
        var passwordHasher = new Mock<IPasswordHasher<User>>();
        var handler = new LoginUserCommandHandler(context, passwordHasher.Object);

        var command = new LoginUserCommand("notexist@test.com", "Test@123");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("User Not Found");
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenPasswordIsWrong()
    {
        // Arrange
        var context = GetInMemoryContext();
        var passwordHasher = new Mock<IPasswordHasher<User>>();

        passwordHasher
            .Setup(x => x.VerifyHashedPassword(
                It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(PasswordVerificationResult.Failed);

        context.Users.Add(new User
        {
            Id = Guid.NewGuid(),
            Email = "fayaz@test.com",
            Name = "Fayaz",
            PasswordHash = "hashed_password",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "fayaz@test.com"
        });
        await context.SaveChangesAsync();

        var handler = new LoginUserCommandHandler(context, passwordHasher.Object);
        var command = new LoginUserCommand("fayaz@test.com", "WrongPassword");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("User Not Found");
    }
}