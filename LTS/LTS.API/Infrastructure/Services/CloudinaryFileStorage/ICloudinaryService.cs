namespace LTS.API.Infrastructure.Services.CloudinaryFileStorage
{
    public interface ICloudinaryService
    {
        Task<FileUploadResult> UploadFileAsync(IFormFile file);
        Task<bool> DeleteFileAsync(string publicId,string fileType);
    }
}
