using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
namespace LTS.API.Infrastructure.Services.CloudinaryFileStorage
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryService(IOptions<CloudinarySettings> options)
        {
            var settings = options.Value;
            var account = new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret);
            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true; // Ensure secure URL
        }
        public async Task<FileUploadResult> UploadFileAsync(IFormFile file)
        {
            string folderName = "lts";
            FileValidationHelper.Validate(file);

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            return FileValidationHelper.IsImage(extension)
                ? await UploadImageAsync(file, folderName)
                : await UploadRawFileAsync(file, folderName);
        }
        public async Task<bool> DeleteFileAsync(string publicId, string fileType)
        {
            if (string.IsNullOrWhiteSpace(publicId))
                throw new Exception("PublicId is required.");

            var resourceType = fileType.StartsWith("image/", StringComparison.OrdinalIgnoreCase)
                ? ResourceType.Image
                : ResourceType.Raw;

            var deleteParams = new DeletionParams(publicId)
            {
                ResourceType = resourceType
            };

            var result = await _cloudinary.DestroyAsync(deleteParams);
            return result.Result == "ok";
        }

        private async Task<FileUploadResult> UploadImageAsync(IFormFile file, string folderName)
        {
            await using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folderName,
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception($"Cloudinary image upload failed: {uploadResult.Error?.Message}");

            return new FileUploadResult
            {
                Url = uploadResult.SecureUrl.ToString(),
                PublicId = uploadResult.PublicId
            };
        }

        private async Task<FileUploadResult> UploadRawFileAsync(IFormFile file, string folderName)
        {
            await using var stream = file.OpenReadStream();

            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folderName,
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception($"Cloudinary file upload failed: {uploadResult.Error?.Message}");

            // PDF ke liye URL mein fl_attachment add karo
            var url = uploadResult.SecureUrl.ToString();
            if (Path.GetExtension(file.FileName).ToLowerInvariant() == ".pdf")
            {
                url = url.Replace("/raw/upload/", "/raw/upload/fl_attachment/");
            }

            return new FileUploadResult
            {
                Url = url,
                PublicId = uploadResult.PublicId
            };
        }

    }
}
