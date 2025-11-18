using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public class OfficeAttendance {
    public OfficeAttendance(int userID, DateOnly? date, OfficeAttendanceStatus status) {
        this.UserID = userID;
        this.Date = date;
        this.Status = status;
    }
    
    [Key]
    [Column(Order = 0)]
    public int ID { get; set; }
    [Column(Order = 1)]
    public int UserID { get; set; }
    public User User { get; set; }
    [Column(Order = 2)]
    public DateOnly? Date { get; set; }
    [Column(Order = 3)]
    public OfficeAttendanceStatus Status { get; set; }
}

public enum OfficeAttendanceStatus {
    Attending,
    Absent
}
