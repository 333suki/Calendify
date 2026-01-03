using Backend.Dtos;
using Backend.Models;

namespace Backend.Services;

public interface IRoomBookingService
{
    IEnumerable<RoomBooking> GetBooking(int id, bool isAdmin);

    bool RoomExists(int? id);

    bool BookingOverlap(int? roomId, DateTime? startTime, DateTime? endTime);

    RoomBooking CreateBooking(int userID, NewRoomBookingRequest? req);

    bool DeleteBooking(int id);

    bool UpdateBooking(int id, UpdateRoomBookingRequest? req);
 
}