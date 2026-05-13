using LTS.API.Common.Response;
using LTS.API.Features.UserManangement.DTOs;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LTS.API.Features.UserManangement.Queries.GetProfiles
{
    public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, ApiResponse<GetProfileDto>>
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetProfileQueryHandler(AppDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiResponse<GetProfileDto>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?
                 .User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return ApiResponse<GetProfileDto>.Fail("Unauthorized");

            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId), cancellationToken);

            if (user == null)
                return ApiResponse<GetProfileDto>.Fail("User not found");

            return ApiResponse<GetProfileDto>.Ok(new GetProfileDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phonenumber,
                Location = user.Location,
                ProfilePictureUrl = user.ProfileImageUrl,
                JoinedAt = user.CreatedAt,
            });
        }
    }
}
