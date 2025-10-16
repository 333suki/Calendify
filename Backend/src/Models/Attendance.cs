using Backend.Models;

public class Attendance
{
    public int UserID { get; set; }
    public User User { get; set; }

    public int EventID { get; set; }
    public Event Event { get; set; }
}