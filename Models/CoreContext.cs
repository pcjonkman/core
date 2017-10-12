using Microsoft.EntityFrameworkCore;
 
namespace Core.Models
{
    public class CoreContext : DbContext
    {
        public CoreContext(DbContextOptions<CoreContext> options)
            : base(options)
        {
        }
 
        public DbSet<User> Users { get; set; }
 
        public DbSet<Post> Posts { get; set; }
    }
}