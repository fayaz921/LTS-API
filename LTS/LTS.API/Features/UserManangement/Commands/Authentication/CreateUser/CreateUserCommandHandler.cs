using LTS.API.Domain.Entities;
using LTS.API.Domain.Enums;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.UserManangement.Commands.Authentication.CreateUser
{
    public class CreateUserCommandHandler: IRequestHandler<CreateUserCommand, string>
    {
        private readonly AppDbContext _context;

        public CreateUserCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var emailExists = await _context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);

            if (emailExists)
            {
                return "Email Exists";
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.FullName,
                Email = request.Email,
                Role = UserRole.User,
                IsActive = false,
                CreatedAt = DateTime.UtcNow,
                PasswordHash = 779797897987.GetHashCode().ToString().Select(c => (byte)c).ToArray() // Placeholder for actual password hashing
                
            };


            //    await _context.Users.AddAsync(user, cancellationToken);
            //    await _context.UserCredentials.AddAsync(credential, cancellationToken);
            //    await _context.SaveChangesAsync(cancellationToken);
                return "User Created";
        }
    }
}
