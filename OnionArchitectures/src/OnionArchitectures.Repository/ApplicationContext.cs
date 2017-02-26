using Microsoft.EntityFrameworkCore;
using OnionArchitectures.Data;


namespace OnionArchitectures.Repository
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var userMap = new UserMap(modelBuilder.Entity<User>());
            var userProfileMap = new UserProfileMap(modelBuilder.Entity<UserProfile>());
        }
    }
}
