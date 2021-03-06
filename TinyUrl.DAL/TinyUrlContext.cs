using Microsoft.EntityFrameworkCore;
using TinyUrl.DAL.Models;

namespace TinyUrl.DAL
{
    public class TinyUrlContext:DbContext
    {
        public TinyUrlContext(DbContextOptions options)
            : base(options)
        { }

        public TinyUrlContext()
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Url> Urls { get; set; }
        
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString =  "Server=localhost;Trusted_Connection=True;Database=TinyUrlDataBase;App=EntityFramework;";
                optionsBuilder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => new {e.UserName}).IsUnique();
            });

            modelBuilder.Entity<Url>(entity =>
            {
                entity.HasIndex(e => new {e.TinyPath}).IsUnique();
            });
            
        }
        
        

    }
}