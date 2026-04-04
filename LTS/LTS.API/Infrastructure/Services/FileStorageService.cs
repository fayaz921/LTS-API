using LTS.API.Infrastructure.Services.Interfaces;

namespace LTS.API.Infrastructure.Services
{
    public class FileStorageService : IFileStorageService
    {
        public Task DeleteFileAsync(string filePath)
        {
            throw new NotImplementedException();
        }

        public Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            throw new NotImplementedException();
        }
    }
}
