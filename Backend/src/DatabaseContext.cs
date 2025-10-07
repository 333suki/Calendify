using Backend.Models;
using Microsoft.EntityFrameworkCore;


namespace Backend;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    public DbSet<Models.User> Users { get; set; }
    public DbSet<Models.RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>()
        .HasKey(r => r.ID);

        modelBuilder.Entity<RefreshToken>()
        .HasKey(r => r.ID);

        modelBuilder.Entity<User>()
        .HasMany<RefreshToken>()
        .WithOne(r => r.User)
        .HasForeignKey(r => r.UserID)
        .OnDelete(DeleteBehavior.Cascade);
    }
}



// table for refresh tokens
// events table
// maybe notifs
// 