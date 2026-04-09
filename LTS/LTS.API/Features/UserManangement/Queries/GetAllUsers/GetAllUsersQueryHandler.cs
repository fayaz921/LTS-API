using LTS.API.Common.Response;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.UserManangement.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler: IRequestHandler<GetAllUsersQuery, ApiResponse<List<GetAllUsersDto>>>
    {
        private readonly AppDbContext _context;
        public GetAllUsersQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<List<GetAllUsersDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var organizations = await _context.Organizations
                .Include(o => o.Users)
                .ToListAsync(cancellationToken);

            var result = organizations.Select(o => o.ToDto()).ToList();
            return ApiResponse<List<GetAllUsersDto>>.Ok(result, "All data fetched successfully");
        }
    }
}

