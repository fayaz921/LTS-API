using FluentAssertions;
using LTS.API.Domain.Entities;
using LTS.API.Features.UserManangement.Commands.Authentication.ForgetPassword;
using LTS.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

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

        private ForgetPasswordCommandHandler CreateHandler(AppDbContext context)
        {
            var loggerMock = new Mock<ILogger<ForgetPasswordCommandHandler>>();
            return new ForgetPasswordCommandHandler(context, loggerMock.Object);
        }

        // ✅ SUCCESS CASE
        [Fact]
        public async Task Should_Return_Success_When_Email_Exists()
        {
            var context = GetInMemoryContext();
            context.Users.Add(CreateUser());
            await context.SaveChangesAsync();

            var handler = CreateHandler(context);

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
            var handler = CreateHandler(context);

            var result = await handler.Handle(
                new ForgetPasswordCommand("notfound@test.com"),
                CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("Email not found");
        }

        // ✅ OTP SAVE CHECK
        [Fact]
        public async Task Should_Save_Otp_And_Expiry_When_Email_Exists()
        {
            var context = GetInMemoryContext();
            context.Users.Add(CreateUser());
            await context.SaveChangesAsync();

            var handler = CreateHandler(context);
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
    }
}