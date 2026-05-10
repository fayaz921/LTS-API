using LTS.API.Common.Response;
using LTS.API.Features.UserManangement.DTOs;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LTS.API.Features.UserManangement.Queries.GetMe
{
    public class GetMeQueryHandler : IRequestHandler<GetMeQuery, ApiResponse<GetMeDto>>
    {
        private readonly ILogger<GetMeQueryHandler> _logger;
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetMeQueryHandler(ILogger<GetMeQueryHandler> logger, AppDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiResponse<GetMeDto>> Handle(GetMeQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?
                .User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return ApiResponse<GetMeDto>.Fail("Unauthorized");

            var user = await _db.Users
                .Include(u => u.Organization)
                .FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId), cancellationToken);

            if (user == null)
                return ApiResponse<GetMeDto>.Fail("User not found");

            return ApiResponse<GetMeDto>.Ok(new GetMeDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                ProfileImage = user.ProfileImageUrl,
                OrganizationId = user.OrganizationId,
                OrganizationName = user.Organization.OrganizationName,
                OrganizationPlan = user.Organization.Plan.ToString(),
            });
        
    }
    }
}
