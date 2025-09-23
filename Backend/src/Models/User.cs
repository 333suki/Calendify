using System.ComponentModel.DataAnnotations;

namespace Backend.Models;
public class User
{
    public User(string username, string password)
    {
        this.Username = username;
        this.Password = password;
    }

    [Key]
    public int ID { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

}