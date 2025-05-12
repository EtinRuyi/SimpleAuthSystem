using Microsoft.EntityFrameworkCore;
using SimpleAuthSystem.Domain.Entities;

namespace SimpleAuthSystem.Infrastructure.AppContext
{
    public class AuthSystemContext : DbContext
    {
        public AuthSystemContext(DbContextOptions<AuthSystemContext> options) 
            : base(options){}

        public DbSet<AppUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.HasIndex(e => e.UserName).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });
        }
    }
}
