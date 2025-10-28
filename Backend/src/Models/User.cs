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
    [Column(Order=0)]
    public int ID { get; set; }
    [Column(Order = 1)]
    public string Username { get; set; }
    [Column(Order = 2)]
    public string Email { get; set; }
    [Column(Order = 3)]
    public string Password { get; set; }
    [Column(Order=4)]
    public Role Role { get; set; }

    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
}

public enum Role
{
    Admin,
    User
}