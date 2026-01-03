namespace Backend.Dtos;

public record class NewRoomBookingRequest(int? RoomID, DateTime? StartTime, DateTime? EndTime);
