namespace Backend.Models;

using System.ComponentModel.DataAnnotations.Schema;

public class EventAttendance
{
    public EventAttendance(int userID, int eventID) {
        this.UserID = userID;
        this.EventID = eventID;
    }
    
    [Column(Order = 0)]
    public int UserID { get; set; }
    public User User { get; set; }
    [Column(Order = 1)]
    public int EventID { get; set; }
    public Event Event { get; set; }
}
