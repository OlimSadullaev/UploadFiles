using Microsoft.EntityFrameworkCore;
using TestTask.Entities;
using TestTask.Entities.Enums;
using TestTask.Interfaces;
using TestTask.TaskDbContext;

namespace TestTask.Services
{
    public class UserService : IUserService
    {
        private readonly TestTaskDbContext dbContext;

        public UserService(TestTaskDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddUserAsync(User userCsvData)
        {
            dbContext.Users.Add(userCsvData);
            await dbContext.SaveChangesAsync();
        }

        public async ValueTask<IEnumerable<User>> GetUsersAsync(string sortBy, SortDirection sortDirection, int? limit)
        {
            var query = dbContext.Users.AsQueryable();

            switch (sortBy.ToLower())
            {
                case "username":
                    query = sortDirection == SortDirection.Ascending
                        ? query.OrderBy(u => u.UserName)
                        : query.OrderByDescending(u => u.UserName);
                    break;
                // Add more cases for other properties if needed
                default:
                    // Default to sorting by username
                    query = query.OrderBy(u => u.UserName);
                    break;
            }

            if (limit.HasValue)
            {
                query = query.Take(limit.Value);
            }

            return await query.ToListAsync();
        }

        public async Task UpdateOrAddUserAsync(User userData)
        {
            var existingUser = await dbContext.Users.FirstOrDefaultAsync(
                u => u.UserIdentifier == userData.UserIdentifier);

            if (existingUser != null)
            {
                // Update existing user with new data
                existingUser.UserName = userData.UserName;
                existingUser.Age = userData.Age;
                existingUser.City = userData.City;
                existingUser.PhoneNumber = userData.PhoneNumber;
                existingUser.Email = userData.Email;
            }
            else
            {
                // Add new user
                var newUser = new User
                {
                    UserIdentifier = userData.UserIdentifier,
                    UserName = userData.UserName,
                    Age = userData.Age,
                    City = userData.City,
                    PhoneNumber = userData.PhoneNumber,
                    Email = userData.Email
                };
                dbContext.Users.Add(newUser);
            }

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Handled database update error
                throw new Exception("Error updating/adding user in the database.", ex);
            }
        }

        public async Task<bool> UserExistsAsync(long userIdentifier) =>
            await dbContext.Users.AnyAsync(u => u.UserIdentifier == userIdentifier);
    }
}
