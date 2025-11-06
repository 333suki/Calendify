namespace Backend.Models;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class RoomBooking {
    public RoomBooking(int roomID, int userID, DateTime? startTime, DateTime? endTime) {
        this.RoomID = roomID;
        this.UserID = userID;
        this.StartTime = startTime;
        this.EndTime = endTime;
    }
    
    [Key]
    [Column(Order = 0)]
    public int ID { get; set; }
    [Column(Order = 1)]
    public int RoomID { get; set; }
    public Room Room { get; set; }
    [Column(Order = 2)]
    public int UserID { get; set; }
    public User User { get; set; }
    [Column(Order = 3)]
    public DateTime? StartTime { get; set; }
    [Column(Order = 4)]
    public DateTime? EndTime { get; set; }
}
