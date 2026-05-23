using LTS.API.Common.Response;
using LTS.API.Features.UserManangement.DTOs;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LTS.API.Features.UserManangement.Queries.GetProfiles
{
    public sealed class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, ApiResponse<GetProfileDto>>
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetProfileQueryHandler(AppDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _context = db;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiResponse<GetProfileDto>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?
                 .User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userId, out var parsedUserId))
            {
                return ApiResponse<GetProfileDto>.Fail("Unauthorized");
            }
            var profile = await _context.Users
                         .AsNoTracking()
                         .Where(x => x.Id == parsedUserId)
                         .Select(x => new GetProfileDto
                         {
                             Id = x.Id,
                             Name = x.Name,
                             Email = x.Email,
                             Phone = x.Phonenumber,
                             Location = x.Location,
                             ProfilePictureUrl = x.ProfileImageUrl,
                             JoinedAt = x.CreatedAt
                         })
                         .FirstOrDefaultAsync(cancellationToken);
            if (profile is null)
            {
                return ApiResponse<GetProfileDto>.Fail("User not found");
            }

            return ApiResponse<GetProfileDto>.Ok(
                profile,
                "Profile fetched successfully");
        }
    }
}
