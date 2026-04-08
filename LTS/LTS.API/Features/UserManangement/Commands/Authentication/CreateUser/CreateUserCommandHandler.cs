using LTS.API.Common.Response;
using LTS.API.Domain.Entities;
using LTS.API.Domain.Enums;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace LTS.API.Features.UserManangement.Commands.Authentication.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApiResponse<string>>
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        public CreateUserCommandHandler(AppDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<ApiResponse<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
            if (user != null)
            {
                return ApiResponse<string>.Fail("Email already Exist");
            }
            var organization = new Organization
            {
                Id = Guid.NewGuid(),
                OrganizationName = request.OrganizationName,
                Slug = request.OrganizationName.ToLower().Trim().Replace(" ", "-"),
                Plan = request.SubscriptionPlan,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(1),
                IsActive = false,
                MaxUsers = GetMaxUsers(request.SubscriptionPlan),
                CreatedAt = DateTime.UtcNow,
            };
            var users = new User
            {
                Id = Guid.NewGuid(),
                OrganizationId = organization.Id,
                Name = request.OwnerName,
                Email = request.Email,
                PasswordHash = _passwordHasher.HashPassword(null!, request.Password),
                Role = UserRole.User,
                Otp="",
                IsActive = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = request.Email
            };
            await _context.Organizations.AddAsync(organization, cancellationToken);
            await _context.Users.AddAsync(users, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return ApiResponse<string>.Ok(default!,"Registration successful");
        }
        private int GetMaxUsers(SubscriptionPlan plan) => plan switch
        {
            SubscriptionPlan.Free => 2,
            SubscriptionPlan.Basic => 5,
            SubscriptionPlan.Pro => 20,
            SubscriptionPlan.Enterprise => 100,
            _ => 2
        };
    }
}
