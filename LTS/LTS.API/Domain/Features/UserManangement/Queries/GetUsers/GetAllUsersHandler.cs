using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Domain.Features.UserManangement.Queries.GetUsers
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUserQuery, List<UserGetDto>>
    {
        private readonly AppDbContext _context;
        public GetAllUsersHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<UserGetDto>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            return await _context.Users
                 .Where(u => u.IsActive)
                 .Select(u => new UserGetDto(
                     u.UserId,
                     u.Name,
                     u.Email,
                     u.Role.ToString(),
                     u.IsActive,
                     u.CreatedAt
                 ))
                 .ToListAsync(cancellationToken);
        }
    }
}
