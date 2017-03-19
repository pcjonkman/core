using Microsoft.EntityFrameworkCore;
using Core.Models;
 
namespace Core.Context
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