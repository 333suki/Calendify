using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public class Streak {
    public Streak(int userID, int count) {
        this.UserID = userID;
        this.Count = count;
    }

    [Key]
    [Column(Order = 0)]
    public int ID { get; set; }
    [Column(Order = 1)]
    public int UserID { get; set; }
    public User User { get; set; }
    [Column(Order = 2)]
    public int Count { get; set; }
}
