//using LTS.API.Domain.Entities;
//using LTS.API.Features.UserManangement.Commands.Authentication.LoginUser;
//using LTS.API.Infrastructure.Persistence;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using FluentAssertions;

//namespace LTS.Tests.Features.UserManagement.Commands;

//public class LoginHandlerTests
//{
//    private AppDbContext GetInMemoryContext()
//    {
//        var options = new DbContextOptionsBuilder<AppDbContext>()
//            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//            .Options;
//        return new AppDbContext(options);
//    }

//    private User CreateUser(string email = "fayaz@test.com", string name = "Fayaz", bool isActive = true) => new()
//    {
//        Id = Guid.NewGuid(),
//        Email = email,
//        Name = name,
//        PasswordHash = "hashed_password",
//        IsActive = isActive,
//        CreatedAt = DateTime.UtcNow,
//        CreatedBy = email
//    };

//    private Mock<IPasswordHasher<User>> GetPasswordHasher(PasswordVerificationResult result)
//    {
//        var mock = new Mock<IPasswordHasher<User>>();
//        mock.Setup(x => x.VerifyHashedPassword(
//                It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
//            .Returns(result);
//        return mock;
//    }

//    // ── Handler Tests ──────────────────────────────────────────────────────────

//    [Fact]
//    public async Task Handle_ShouldReturnSuccess_WhenCredentialsAreValid()
//    {
//        // Arrange
//        var context = GetInMemoryContext();
//        context.Users.Add(CreateUser());
//        await context.SaveChangesAsync();

//        var handler = new LoginUserCommandHandler(context, GetPasswordHasher(PasswordVerificationResult.Success).Object);
//        var command = new LoginUserCommand("fayaz@test.com", "Test@123");

//        // Act
//        var result = await handler.Handle(command, CancellationToken.None);

//        // Assert
//        result.IsSuccess.Should().BeTrue();
//        result.Message.Should().Be("Login Success");
//    }

//    [Fact]
//    public async Task Handle_ShouldReturnSuccess_WhenPasswordNeedsRehash()
//    {
//        // Arrange
//        var context = GetInMemoryContext();
//        context.Users.Add(CreateUser());
//        await context.SaveChangesAsync();

//        var handler = new LoginUserCommandHandler(context, GetPasswordHasher(PasswordVerificationResult.SuccessRehashNeeded).Object);
//        var command = new LoginUserCommand("fayaz@test.com", "Test@123");

//        // Act
//        var result = await handler.Handle(command, CancellationToken.None);

//        // Assert
//        // SuccessRehashNeeded bhi valid login hai — handler isko Success maanta hai ya Fail?
//        // Agar handler sirf Failed check karta hai toh yeh pass hoga
//        result.IsSuccess.Should().BeTrue();
//        result.Message.Should().Be("Login Success");
//    }

//    [Fact]
//    public async Task Handle_ShouldFail_WhenUserNotFound()
//    {
//        // Arrange
//        var context = GetInMemoryContext();
//        var handler = new LoginUserCommandHandler(context, GetPasswordHasher(PasswordVerificationResult.Success).Object);

//        var command = new LoginUserCommand("notexist@test.com", "Test@123");

//        // Act
//        var result = await handler.Handle(command, CancellationToken.None);

//        // Assert
//        result.IsSuccess.Should().BeFalse();
//        result.Message.Should().Be("User Not Found");
//    }

//    [Fact]
//    public async Task Handle_ShouldFail_WhenPasswordIsWrong()
//    {
//        // Arrange
//        var context = GetInMemoryContext();
//        context.Users.Add(CreateUser());
//        await context.SaveChangesAsync();

//        var handler = new LoginUserCommandHandler(context, GetPasswordHasher(PasswordVerificationResult.Failed).Object);
//        var command = new LoginUserCommand("fayaz@test.com", "WrongPassword");

//        // Act
//        var result = await handler.Handle(command, CancellationToken.None);

//        // Assert
//        result.IsSuccess.Should().BeFalse();
//        result.Message.Should().Be("User Not Found");
//    }

//    [Fact]
//    public async Task Handle_ShouldUseCorrectUserForPasswordVerification()
//    {
//        // Arrange
//        var context = GetInMemoryContext();
//        var user = CreateUser("fayaz@test.com");
//        context.Users.Add(user);
//        await context.SaveChangesAsync();

//        var passwordHasher = new Mock<IPasswordHasher<User>>();
//        passwordHasher
//            .Setup(x => x.VerifyHashedPassword(
//                It.Is<User>(u => u.Email == "fayaz@test.com"),
//                "hashed_password",
//                "Test@123"))
//            .Returns(PasswordVerificationResult.Success);

//        var handler = new LoginUserCommandHandler(context, passwordHasher.Object);
//        var command = new LoginUserCommand("fayaz@test.com", "Test@123");

//        // Act
//        var result = await handler.Handle(command, CancellationToken.None);

//        // Assert
//        result.IsSuccess.Should().BeTrue();
//        // Verify karta hai ke VerifyHashedPassword sahi user aur password ke saath call hua
//        passwordHasher.Verify(x => x.VerifyHashedPassword(
//            It.Is<User>(u => u.Email == "fayaz@test.com"),
//            "hashed_password",
//            "Test@123"), Times.Once);
//    }

//    // ── Validator Tests ────────────────────────────────────────────────────────

//    [Fact]
//    public void Validator_ShouldPass_WhenCommandIsValid()
//    {
//        // Arrange
//        var validator = new LoginUserCommandValidator();
//        var command = new LoginUserCommand("fayaz@test.com", "Test@123");

//        // Act
//        var result = validator.Validate(command);

//        // Assert
//        result.IsValid.Should().BeTrue();
//    }

//    [Theory]
//    [InlineData("", "Test@123", "Email is required.")]
//    [InlineData("not-an-email", "Test@123", "Invalid email format.")]
//    [InlineData("fayaz@test.com", "", "Password is required.")]
//    [InlineData("fayaz@test.com", "abc", "Password must be at least 6 characters long.")]
//    public void Validator_ShouldFail_WhenInputIsInvalid(string email, string password, string expectedError)
//    {
//        // Arrange
//        var validator = new LoginUserCommandValidator();
//        var command = new LoginUserCommand(email, password);

//        // Act
//        var result = validator.Validate(command);

//        // Assert
//        result.IsValid.Should().BeFalse();
//        result.Errors.Should().Contain(e => e.ErrorMessage == expectedError);
//    }
//}