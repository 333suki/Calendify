namespace Backend.Dtos;

public record class UpdateRoomBookingRequest(int RoomID, int UserID, DateTime StartTime, DateTime EndTime);
