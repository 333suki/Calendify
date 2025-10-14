using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class User
{
    public User(string username, string password, Role role = Role.User)
    {
        this.Username = username;
        this.Password = password;
        this.Role = role;
    }

    [Key]
    public int ID { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }

}


public enum Role
{
    Admin,
    User
}

//role
