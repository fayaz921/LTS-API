using CloudinaryDotNet.Core;
using LTS.API.Common.Response;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Npgsql.Replication.PgOutput.Messages;
using System.Security.Claims;

namespace LTS.API.Features.UserManangement.Commands.UpdateProfies
{
    public class UpdateProfileCommandHandler(AppDbContext _db,IHttpContextAccessor _httpContextAccessor) : IRequestHandler<UpdateProfileCommand, ApiResponse<string>>
    {
        public async Task<ApiResponse<string>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?
            .User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return ApiResponse<string>.Fail("Unauthorized");

            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId), cancellationToken);

            if (user == null)
                return ApiResponse<string>.Fail("User not found");

            user.Name = request.Name;
            user.Phonenumber = request.Phone;
            user.Location = request.Location;
            user.UpdatedAt = DateTime.UtcNow;

            user.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync(cancellationToken);

            return ApiResponse<string>.Ok(default!,"Profile updated successfully");
        }
    }
}
