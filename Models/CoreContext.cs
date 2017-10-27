using System;
using System.Linq;
using Core.Models.Pool;
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

        public DbSet<Country> Country { get; set; }
        public DbSet<Finals> Finals { get; set; }
        public DbSet<FinalsPlacing> FinalsPlacing { get; set; }
        public DbSet<FinalsPrediction> FinalsPrediction { get; set; }
        public DbSet<Match> Match { get; set; }
        public DbSet<MatchFinals> MatchFinals { get; set; }
        public DbSet<MatchPrediction> MatchPrediction { get; set; }
        public DbSet<PoolMessage> PoolMessage { get; set; }
        public DbSet<PoolPlayer> PoolPlayer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Finals>()
                .HasAlternateKey(a => a.LevelNumber);
            modelBuilder.Entity<FinalsPrediction>()
                .Property(p => p.SubScore)
                .HasDefaultValue(0);
            modelBuilder.Entity<MatchPrediction>()
                .Property(p => p.SubScore)
                .HasDefaultValue(0);
            modelBuilder.Entity<PoolPlayer>()
                .Property(p => p.SubScore)
                .HasDefaultValue(0);
        }

        public override int SaveChanges()
        {
            this.ChangeTracker.DetectChanges();

            var entries = this.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
              var property = entry.Properties.SingleOrDefault((prop) => { return prop.Metadata.Name == "LastModified"; } );
              if (property != null) {
                entry.Property("LastModified").CurrentValue = DateTime.UtcNow;
              }
            }

            return base.SaveChanges();
        }

    }
}