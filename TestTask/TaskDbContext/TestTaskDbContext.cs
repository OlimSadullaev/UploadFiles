using Microsoft.EntityFrameworkCore;
using TestTask.Entities;

namespace TestTask.TaskDbContext
{
    public class TestTaskDbContext : DbContext
    {
        public TestTaskDbContext(DbContextOptions<TestTaskDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasKey(u => u.UserIdentifier);
        }

        public DbSet<User> Users { get; set; }
    }
}
