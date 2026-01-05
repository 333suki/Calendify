using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public class User
{
    public User(string username, string email, string password, Role role = Role.User)
    {
        this.Username = username; 
        this.Email = email;
        this.Password = password;
        this.Role = role;
    }

    [Key]
    [Column(Order = 0)]
    public int ID { get; set; }
    [Column(Order = 1)]
    public string Username { get; set; }
    [Column(Order = 2)]
    public string Email { get; set; }
    [Column(Order = 3)]
    public string Password { get; set; }
    [Column(Order=4)]
    public Role Role { get; set; }
    [Column(Order=5)]
    public string? ResetToken { get; set; }
    [Column(Order=6)]
    public DateTime? ResetTokenExpiry { get; set; }

    public ICollection<EventAttendance> EventAttendances { get; set; } = new List<EventAttendance>();
    public ICollection<OfficeAttendance> OfficeAttendances { get; set; } = new List<OfficeAttendance>();
    public ICollection<RoomBooking> RoomBookings { get; set; } = new List<RoomBooking>();
}

public enum Role
{
    Admin,
    User
}
