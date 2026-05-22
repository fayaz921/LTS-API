using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
namespace LTS.API.Infrastructure.Services.CloudinaryFileStorage
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private const string DefaultFolder = "lts";
        public CloudinaryService(IOptions<CloudinarySettings> options)
        {
            var settings = options.Value;
            var account = new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret);
            _cloudinary = new Cloudinary(account)
            {
                Api = { Secure = true }
            };
        }
        public async Task<FileUploadResult> UploadFileAsync(IFormFile file)
        {
            var validationResult = FileValidationHelper.Validate(file);
            if (!validationResult.IsSuccess)
                return validationResult;
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var isImage = FileValidationHelper.IsImage(extension);
            await using var stream = file.OpenReadStream();
            var uploadParams = isImage
                ? BuildImageParams(file, stream)
                : BuildRawParams(file, stream);
            var result = await _cloudinary.UploadAsync(uploadParams);
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
                return new FileUploadResult
                {
                    IsSuccess = false,
                    Message = result.Error?.Message ?? "Upload failed"
                };
            return new FileUploadResult
            {
                IsSuccess = true,
                Message = "File uploaded successfully",
                Url = result.SecureUrl.ToString(),
                PublicId = result.PublicId
            };
        }
        public async Task<bool> DeleteFileAsync(string publicId, string fileType)
        {
            if (string.IsNullOrWhiteSpace(publicId))
                 return false;
            var resourceType = fileType.StartsWith("image/", StringComparison.OrdinalIgnoreCase)
                ? ResourceType.Image
                : ResourceType.Raw;
            var deleteParams = new DeletionParams(publicId)
            {
                ResourceType = resourceType
            };
            var result = await _cloudinary.DestroyAsync(deleteParams);
            if (result.Result == "not found")
                return false;

            if (result.Result == "error")
                return false;

            return result.Result == "ok";
        }
        private ImageUploadParams BuildImageParams(IFormFile file, Stream stream)
        {
            return new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = DefaultFolder,
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false
            };
        }
        private RawUploadParams BuildRawParams(IFormFile file, Stream stream)
        {
            return new RawUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = DefaultFolder,
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false
            };
        }
    }
}
