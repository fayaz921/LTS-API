using LTS.API.Common.Response;
using LTS.API.Domain.Entities;
using LTS.API.Features.UserManangement.Mappers;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
            var existingUser = await _context.Users
          .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
            if (existingUser != null)
                return ApiResponse<string>.Fail("Email already exists");
            var organization = request.ToOrganization();
            var passwordHash = _passwordHasher.HashPassword(null!, request.Password);
            var user = request.ToUser(organization.Id, passwordHash);
            await _context.Organizations.AddAsync(organization, cancellationToken);
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return ApiResponse<string>.Ok(default!, "Registration successful");
        }
    }
}
