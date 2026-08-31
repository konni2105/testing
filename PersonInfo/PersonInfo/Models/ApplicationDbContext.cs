using Microsoft.EntityFrameworkCore;

namespace PersonInfo.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<UserInfoAccount> UserInfoAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);   //in this method we can use fluent API to set tables and column properties like lenght or even table relationship.
        }
    }
}
