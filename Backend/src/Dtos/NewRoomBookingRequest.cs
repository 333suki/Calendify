namespace Backend.Dtos;


public class NewRoomBookingRequest {
    public int RoomID { get; set; }
    public int UserID { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}