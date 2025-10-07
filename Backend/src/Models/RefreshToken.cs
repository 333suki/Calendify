using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class RefreshToken
{
    public RefreshToken(string token, int userID)
    {
        this.Token = token;
        this.UserID = userID;
    }

    [Key]
    public int ID { get; set; }
    public int UserID { get; set; }
    public string Token { get; set; }

    // nav property
    public User User { get; set; }
}
