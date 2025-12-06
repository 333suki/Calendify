namespace Backend.Dtos;

public record class UpdateRoomBookingRequest(int roomID, int userID, DateTime starttime, DateTime endtime);


