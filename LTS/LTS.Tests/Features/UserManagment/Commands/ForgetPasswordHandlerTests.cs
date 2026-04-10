using FluentAssertions;
using LTS.API.Domain.Entities;
using LTS.API.Features.UserManangement.Commands.Authentication.ForgetPassword;
using LTS.API.Infrastructure.Persistence;
using LTS.API.Infrastructure.Services.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace LTS.Tests.Features.UserManagment.Commands
{
    public class ForgetPasswordHandlerTests
    {
        private AppDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        private User CreateUser(string email = "fayaz@test.com", string name = "Fayaz") => new()
        {
            Id = Guid.NewGuid(),
            Email = email,
            Name = name,
            PasswordHash = "hashed_password",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = email
        };

        private (ForgetPasswordCommandHandler handler, Mock<IEmailService> emailMock)
            CreateHandler(AppDbContext context, bool emailResult = true)
        {
            var emailMock = new Mock<IEmailService>();

            emailMock.Setup(x => x.ForgetPasswordOtp(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(emailResult);

            var loggerMock = new Mock<ILogger<ForgetPasswordCommandHandler>>();

            var handler = new ForgetPasswordCommandHandler(
                context,
                emailMock.Object,
                loggerMock.Object);

            return (handler, emailMock);
        }

        // ✅ SUCCESS CASE
        [Fact]
        public async Task Should_Return_Success_When_Email_Exists_And_Email_Sent()
        {
            var context = GetInMemoryContext();
            context.Users.Add(CreateUser());
            await context.SaveChangesAsync();

            var (handler, _) = CreateHandler(context, true);

            var result = await handler.Handle(
                new ForgetPasswordCommand("fayaz@test.com"),
                CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Contain("OTP sent");
        }

        // ❌ EMAIL NOT FOUND
        [Fact]
        public async Task Should_Return_Error_When_Email_Not_Found()
        {
            var context = GetInMemoryContext();
            var (handler, emailMock) = CreateHandler(context);

            var result = await handler.Handle(
                new ForgetPasswordCommand("notfound@test.com"),
                CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Email not found");

            // email service call nahi honi chahiye
            emailMock.Verify(x => x.ForgetPasswordOtp(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()), Times.Never);
        }

        // ❌ EMAIL SERVICE FAIL
        [Fact]
        public async Task Should_Return_Error_When_Email_Service_Fails()
        {
            var context = GetInMemoryContext();
            context.Users.Add(CreateUser());
            await context.SaveChangesAsync();

            var (handler, _) = CreateHandler(context, false);

            var result = await handler.Handle(
                new ForgetPasswordCommand("fayaz@test.com"),
                CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("Failed to send OTP");
        }

        // ✅ OTP SAVE CHECK
        [Fact]
        public async Task Should_Save_Otp_And_Expiry_When_Email_Exists()
        {
            var context = GetInMemoryContext();
            context.Users.Add(CreateUser());
            await context.SaveChangesAsync();

            var (handler, _) = CreateHandler(context);

            var before = DateTime.UtcNow;

            await handler.Handle(
                new ForgetPasswordCommand("fayaz@test.com"),
                CancellationToken.None);

            var user = await context.Users.FirstAsync();

            user.Otp.Should().NotBeNullOrEmpty();
            user.OTPExpiry.Should().NotBeNull();

            user.OTPExpiry.Should().BeAfter(before);
            user.OTPExpiry.Should().BeBefore(DateTime.UtcNow.AddMinutes(11));
        }

        // ✅ EMAIL SERVICE CALLED
        [Fact]
        public async Task Should_Call_Email_Service_With_Correct_Email_And_Name()
        {
            var context = GetInMemoryContext();
            context.Users.Add(CreateUser("fayaz@test.com", "Fayaz"));
            await context.SaveChangesAsync();

            var (handler, emailMock) = CreateHandler(context);

            await handler.Handle(
                new ForgetPasswordCommand("fayaz@test.com"),
                CancellationToken.None);

            emailMock.Verify(x => x.ForgetPasswordOtp(
                "fayaz@test.com",
                "Fayaz",
                It.IsAny<string>()), Times.Once);
        }
    }
}
