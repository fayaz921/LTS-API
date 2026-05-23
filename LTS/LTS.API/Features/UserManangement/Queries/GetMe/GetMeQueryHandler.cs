using LTS.API.Common.Response;
using LTS.API.Features.UserManangement.DTOs;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LTS.API.Features.UserManangement.Queries.GetMe
{
    public sealed class GetMeQueryHandler : IRequestHandler<GetMeQuery, ApiResponse<GetMeDto>>
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetMeQueryHandler(AppDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _context = db;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiResponse<GetMeDto>> Handle(GetMeQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?
                .User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out var parsedUserId))
                return ApiResponse<GetMeDto>.Fail("Unauthorized");

            var user = await _context.Users
           .AsNoTracking()
           .Where(u => u.Id == parsedUserId)
           .Select(u => new GetMeDto
           {
               Id = u.Id,
               Name = u.Name,
               Email = u.Email,
               ProfileImage = u.ProfileImageUrl,
               OrganizationId = u.OrganizationId,
               OrganizationName = u.Organization.OrganizationName,  // no Include needed
               Role = u.Role.ToString(),
               OrganizationPlan = u.Organization.Plan.ToString(),
           })
           .FirstOrDefaultAsync(cancellationToken);


            if (user == null)
                return ApiResponse<GetMeDto>.Fail("User not found");

            return ApiResponse<GetMeDto>.Ok(user, "User fetched successfully");

        }
    }
}
