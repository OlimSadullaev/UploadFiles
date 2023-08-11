using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using TestTask.Entities;
using TestTask.Interfaces;
using TestTask.TaskDbContext;

namespace TestTask.Services
{
    public class UploadService : IUploadService
    {
        private readonly TestTaskDbContext dbContext;

        public UploadService(TestTaskDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async ValueTask<IEnumerable<User>> ParseCsvDataAsync(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                // Map CSV columns to User properties
                csv.Context.RegisterClassMap<UserCsvMap>();

                var records = csv.GetRecords<User>().ToList(); // Convert to a List
                return records;
            }
        }

        public async Task<string> SaveFileAsync(IFormFile file)
        {
            // Generate a unique file name
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        // Save the file to a directory (adjust path as needed)
        var filePath = Path.Combine("C:\\Desktop\\Upload", fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

            return filePath;
        }

        private sealed class UserCsvMap : ClassMap<User>
        {
            public UserCsvMap()
            {
                Map(m => m.UserName).Name("username");
                Map(m => m.UserIdentifier).Name("useridentifier");
                Map(m => m.Age).Name("age");
                Map(m => m.City).Name("city");
                Map(m => m.PhoneNumber).Name("phonenumber");
                Map(m => m.Email).Name("email");
            }
        }
    }
}
