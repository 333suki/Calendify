namespace Backend.Dtos;

public record class NewRoomBookingRequest(int? RoomID, int? UserID, DateTime? StartTime, DateTime? EndTime);
