namespace LTS.API.Infrastructure.Services.CloudinaryFileStorage
{
    public class FileValidationHelper
    {

        public static readonly string[] ImageExtensions =
        {
            ".jpg", ".jpeg", ".png", ".webp"
        };

        public static readonly string[] DocumentExtensions =
        {
            ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".txt", ".csv"
        };

        private const long OneMB = 1024 * 1024;
        private const long MaxImageSize = 5 * OneMB;     // 5 MB
        private const long MaxDocumentSize = 10 * OneMB; // 10 MB

        public static bool IsImage(string extension)
            => ImageExtensions.Contains(extension);

        public static bool IsDocument(string extension)
            => DocumentExtensions.Contains(extension);

        public static FileUploadResult Validate(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return new FileUploadResult { IsSuccess = false, Message = "No file uploaded." };

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!IsImage(extension) && !IsDocument(extension))
                return new FileUploadResult { IsSuccess = false, Message = "Invalid file type." };

            if (IsImage(extension) && file.Length > MaxImageSize)
                return new FileUploadResult { IsSuccess = false, Message = "Image size cannot exceed 5 MB." };

            if (IsDocument(extension) && file.Length > MaxDocumentSize)
                return new FileUploadResult { IsSuccess = false, Message = "Document size cannot exceed 10 MB." };

            return new FileUploadResult { IsSuccess = true };
        }
    }
}
