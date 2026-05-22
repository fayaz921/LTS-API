using LTS.API.Common.Response;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LTS.API.Features.UserManangement.Commands.UpdateProfies
{
    public class UpdateProfileCommandHandler(AppDbContext _db,IHttpContextAccessor _httpContextAccessor) : IRequestHandler<UpdateProfileCommand, ApiResponse<string>>
    {
        public async Task<ApiResponse<string>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var userIdstring = _httpContextAccessor.HttpContext?
            .User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdstring, out var userId))
                return ApiResponse<string>.Fail("Unauthorized");
            var utcNow = DateTime.UtcNow;
            var affectedRows = await _db.Users
                .Where(x => x.Id == userId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(x => x.Name, request.Name)
                    .SetProperty(x => x.Phonenumber, request.Phone)
                    .SetProperty(x => x.Location, request.Location)
                    .SetProperty(x => x.UpdatedAt, utcNow),
                    cancellationToken);
            if (affectedRows == 0)
                return ApiResponse<string>.Fail("User not found");

            return ApiResponse<string>.Ok(default!,"Profile updated successfully");
        }
    }
}
