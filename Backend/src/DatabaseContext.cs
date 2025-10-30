using Backend.Models;
using Microsoft.EntityFrameworkCore;


namespace Backend;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    public DbSet<Models.User> Users { get; set; }
    public DbSet<Models.RefreshToken> RefreshTokens { get; set; }
    public DbSet<Models.Event> Events { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasKey(u => u.ID);

        modelBuilder.Entity<RefreshToken>()
            .HasKey(r => r.ID);

        modelBuilder.Entity<Event>()
            .HasKey(e => e.ID);

        modelBuilder.Entity<User>()
            .HasMany<RefreshToken>()
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Attendance>()
            .HasKey(a => new { a.UserID, a.EventID });

        modelBuilder.Entity<Attendance>()
            .HasOne(a => a.User)
            .WithMany(u => u.Attendances)
            .HasForeignKey(a => a.UserID);

        modelBuilder.Entity<Attendance>()
            .HasOne(a => a.Event)
            .WithMany(e => e.Attendances)
            .HasForeignKey(a => a.EventID);

    }
}

// table for refresh tokens
// events table


// maybe notifs
// 