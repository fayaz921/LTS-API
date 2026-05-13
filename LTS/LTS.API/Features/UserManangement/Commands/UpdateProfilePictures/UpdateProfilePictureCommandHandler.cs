using LTS.API.Common.Response;
using LTS.API.Infrastructure.Persistence;
using LTS.API.Infrastructure.Services.CloudinaryFileStorage;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LTS.API.Features.UserManangement.Commands.UpdateProfilePictures
{
    public class UpdateProfilePictureCommandHandler : IRequestHandler<UpdateProfilePictureCommand, ApiResponse<string>>
    {
        private readonly AppDbContext _db;
        private readonly ICloudinaryService _cloudinary;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateProfilePictureCommandHandler(AppDbContext db, ICloudinaryService cloudinary, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _cloudinary = cloudinary;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<string>> Handle(UpdateProfilePictureCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?
                .User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return ApiResponse<string>.Fail("Unauthorized");

            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId), cancellationToken);

            if (user == null)
                return ApiResponse<string>.Fail("User not found");

            var uploadResult = await _cloudinary.UploadFileAsync(request.File);

            user.ProfileImageUrl = uploadResult.Url;
            user.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync(cancellationToken);

            return ApiResponse<string>.Ok(uploadResult.Url, "Profile picture updated");
        }
    }
}
