using FluentAssertions;
using LTS.API.Domain.Entities;
using LTS.API.Features.UserManangement.Commands.Authentication.VerifyEmail;
using LTS.API.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace LTS.Tests.Features.UserManagment.Commands
{
    public class VerifyEmailHandlerTest
    {
        private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
        private readonly Mock<ILogger<VerifyEmailCommandHandler>> _loggerMock;

        public VerifyEmailHandlerTest()
        {
            _passwordHasherMock = new Mock<IPasswordHasher<User>>();
            _loggerMock = new Mock<ILogger<VerifyEmailCommandHandler>>();
        }

        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // ✅ always unique
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task Should_Return_Error_When_Email_Not_Found()
        {
            var context = GetDbContext();

            var handler = new VerifyEmailCommandHandler(
                context,
                _passwordHasherMock.Object,
                _loggerMock.Object);

            var command = new VerifyEmailCommand(
                "test@test.com",
                "1234",
                "Password123");

            var result = await handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Email not found");
        }

        [Fact]
        public async Task Should_Return_Error_When_Otp_Is_Invalid()
        {
            var context = GetDbContext();

            context.Users.Add(new User
            {
                Email = "test@test.com",
                Otp = "9999",
                OTPExpiry = DateTime.UtcNow.AddMinutes(5)
            });

            await context.SaveChangesAsync();

            var handler = new VerifyEmailCommandHandler(
                context,
                _passwordHasherMock.Object,
                _loggerMock.Object);

            var command = new VerifyEmailCommand(
                "test@test.com",
                "1234",
                "Password123");

            var result = await handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Invalid OTP");
        }

        [Fact]
        public async Task Should_Return_Error_When_Otp_Is_Expired()
        {
            var context = GetDbContext();

            context.Users.Add(new User
            {
                Email = "test@test.com",
                Otp = "1234",
                OTPExpiry = DateTime.UtcNow.AddMinutes(-1) // expired
            });

            await context.SaveChangesAsync();

            var handler = new VerifyEmailCommandHandler(
                context,
                _passwordHasherMock.Object,
                _loggerMock.Object);

            var command = new VerifyEmailCommand(
                "test@test.com",
                "1234",
                "Password123");

            var result = await handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("expired");
        }

        [Fact]
        public async Task Should_Reset_Password_When_Data_Is_Valid()
        {
            var context = GetDbContext();

            var user = new User
            {
                Email = "test@test.com",
                Otp = "1234",
                OTPExpiry = DateTime.UtcNow.AddMinutes(5)
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            _passwordHasherMock
                .Setup(x => x.HashPassword(It.IsAny<User>(), It.IsAny<string>()))
                .Returns("hashed-password");

            var handler = new VerifyEmailCommandHandler(
                context,
                _passwordHasherMock.Object,
                _loggerMock.Object);

            var command = new VerifyEmailCommand(
                "test@test.com",
                "1234",
                "Password123");

            var result = await handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Contain("successful");

            var updatedUser = await context.Users.FirstAsync();

            updatedUser.PasswordHash.Should().Be("hashed-password");
            updatedUser.Otp.Should().BeEmpty();
            updatedUser.OTPExpiry.Should().BeNull();
        }

        [Fact]
        public async Task Should_Clear_Otp_After_Success()
        {
            var context = GetDbContext();

            var user = new User
            {
                Email = "test@test.com",
                Otp = "1234",
                OTPExpiry = DateTime.UtcNow.AddMinutes(5)
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            _passwordHasherMock
                .Setup(x => x.HashPassword(It.IsAny<User>(), It.IsAny<string>()))
                .Returns("hashed-password");

            var handler = new VerifyEmailCommandHandler(
                context,
                _passwordHasherMock.Object,
                _loggerMock.Object);

            var command = new VerifyEmailCommand(
                "test@test.com",
                "1234",
                "Password123");

            await handler.Handle(command, CancellationToken.None);

            var updatedUser = await context.Users.FirstAsync();

            updatedUser.Otp.Should().Be("");
            updatedUser.OTPExpiry.Should().BeNull();
        }

    }
}
