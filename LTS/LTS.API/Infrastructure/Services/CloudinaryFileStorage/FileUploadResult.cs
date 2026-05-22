namespace LTS.API.Infrastructure.Services.CloudinaryFileStorage
{
    public class FileUploadResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string PublicId { get; set; } = string.Empty;
    }
}
