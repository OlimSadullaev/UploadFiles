using TestTask.Entities;
using TestTask.Entities.Enums;

namespace TestTask.Interfaces
{
    public interface IUserService
    {
        Task<bool> UserExistsAsync(long userIdentifier);
        Task UpdateOrAddUserAsync(User userData);
        Task AddUserAsync(User userCsvData);
        ValueTask<IEnumerable<User>> GetUsersAsync(string sortBy, SortDirection sortDirection, int? limit);
    }
}
