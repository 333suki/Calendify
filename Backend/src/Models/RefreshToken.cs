using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public class RefreshToken
{
    public RefreshToken(string token, int userID)
    {
        this.Token = token;
        this.UserID = userID;
    }

    [Key]
    [Column(Order = 0)]
    public int ID { get; set; }
    [Column(Order = 1)]
    public int UserID { get; set; }
    public User User { get; set; }
    [Column(Order = 2)]
    public string Token { get; set; }
}
