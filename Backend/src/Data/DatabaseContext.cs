using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}

    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<EventAttendance> EventAttendances { get; set; }
    public DbSet<OfficeAttendance> OfficeAttendances { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<RoomBooking> RoomBookings { get; set; }
    public DbSet<Streak> Streaks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        // == User ==
        modelBuilder.Entity<User>()
            .HasKey(u => u.ID);

        // == Refresh token ==
        modelBuilder.Entity<RefreshToken>()
            .HasKey(rt => rt.ID);
        
        modelBuilder.Entity<RefreshToken>()
            .HasOne(rt => rt.User)
            .WithMany()
            .HasForeignKey(rt => rt.UserID)
            .OnDelete(DeleteBehavior.Cascade);

        // == Event ==
        modelBuilder.Entity<Event>()
            .HasKey(e => e.ID);
        
        // == Event attendance ==
        modelBuilder.Entity<EventAttendance>()
            .HasKey(a => new { a.UserID, a.EventID });

        modelBuilder.Entity<EventAttendance>()
            .HasOne(ea => ea.User)
            .WithMany(u => u.EventAttendances)
            .HasForeignKey(ea => ea.UserID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EventAttendance>()
            .HasOne(ea => ea.Event)
            .WithMany(e => e.EventAttendances)
            .HasForeignKey(ea => ea.EventID)
            .OnDelete(DeleteBehavior.Cascade);

        // == Office attendance ==
        modelBuilder.Entity<OfficeAttendance>()
            .HasKey(oa => oa.ID);

        modelBuilder.Entity<OfficeAttendance>()
            .HasOne(oa => oa.User)
            .WithMany(u => u.OfficeAttendances)
            .HasForeignKey(oa => oa.UserID)
            .OnDelete(DeleteBehavior.Cascade);

        // == Room ==
        modelBuilder.Entity<Room>()
            .HasKey(r => r.ID);

        // == Room booking ==
        modelBuilder.Entity<RoomBooking>()
            .HasKey(rb => rb.ID);

        modelBuilder.Entity<RoomBooking>()
            .HasOne(rb => rb.Room)
            .WithMany(r => r.RoomBookings)
            .HasForeignKey(rb => rb.RoomID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RoomBooking>()
            .HasOne(rb => rb.User)
            .WithMany(u => u.RoomBookings)
            .HasForeignKey(rb => rb.UserID)
            .OnDelete(DeleteBehavior.Cascade);
        
        // == Streak ==
        modelBuilder.Entity<Streak>()
            .HasKey(s => s.ID);

        modelBuilder.Entity<Streak>()
            .HasOne(s => s.User)
            .WithOne(u => u.Streak)
            .HasForeignKey<Streak>(s => s.UserID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
