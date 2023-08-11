using TestTask.Entities;

namespace TestTask.Interfaces
{
    public interface IUploadService
    {
        Task<string> SaveFileAsync(IFormFile file);
        ValueTask<IEnumerable<User>> ParseCsvDataAsync(string filePath);
    }
}
