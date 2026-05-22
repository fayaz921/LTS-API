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
            var userIdstring = _httpContextAccessor.HttpContext?
                .User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdstring, out var userId))
                return ApiResponse<string>.Fail("Unauthorized");
            if (!IsValidImage(request.File))
                return ApiResponse<string>.Fail("Only image files are allowed (jpg, jpeg, png, webp)");
            var utcNow = DateTime.UtcNow;
            var user = await _db.Users
                .AsNoTracking()
                .Where(x => x.Id == userId)
                .Select(x => new
                {
                    x.Id,
                    x.ProfileImagePublicId
                })
                .FirstOrDefaultAsync(cancellationToken);
            if (user == null)
                return ApiResponse<string>.Fail("User not found");
            var oldPublicId = user.ProfileImagePublicId;
            var uploadResult = await _cloudinary.UploadFileAsync(request.File);
            if (!uploadResult.IsSuccess)
                return ApiResponse<string>.Fail(uploadResult.Message);
            var affectedRows = await _db.Users
            .Where(x => x.Id == userId)
            .ExecuteUpdateAsync(setters => setters
              .SetProperty(x => x.ProfileImageUrl, uploadResult.Url)
              .SetProperty(x => x.ProfileImagePublicId, uploadResult.PublicId)
              .SetProperty(x => x.UpdatedAt, utcNow),
              cancellationToken);

            if (affectedRows == 0)
                return ApiResponse<string>.Fail("Profile update failed");

            if (!string.IsNullOrWhiteSpace(oldPublicId))
            {
                await _cloudinary.DeleteFileAsync(
                    oldPublicId,
                    "image/"
                );
            }
            return ApiResponse<string>.Ok(
                uploadResult.Url,
                "Profile picture updated successfully");
        }
        private static bool IsValidImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            return allowedExtensions.Contains(extension);
        }
    }
}
