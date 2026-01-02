using System.Linq.Expressions;
using Backend.Dtos;
using Backend.Models;

namespace Backend.Services;

public class RoomBookingService : IRoomBookingService
{
    private readonly IRepository<RoomBooking> _roomBookingrepo;
    private readonly IRepository<Room> _roomrepo;


    public RoomBookingService(IRepository<RoomBooking> repository)
    {
        _roomBookingrepo = repository;
    }

    public IEnumerable<RoomBooking> GetBooking(int id, bool isAdmin)
    {
        if (isAdmin)
        {
            return _roomBookingrepo.GetAll();
        }

        return _roomBookingrepo.GetBy(b => b.UserID == id);
    
    }

    public bool BookingOverlap(int? roomId, DateTime? startTime, DateTime? endTime)
    {
        return _roomBookingrepo
            .GetBy(b =>
                b.RoomID == roomId &&
                b.StartTime < endTime &&
                startTime < b.EndTime
            )
            .Any();
    }

    public bool RoomExists(int? id)
    {
        Room room = _roomrepo.GetBy(b => b.ID == id).FirstOrDefault();
        if (room == null)
        {
            return false;
        }
        return true;
    }

    public RoomBooking CreateBooking(NewRoomBookingRequest req)
    {
        if (BookingOverlap(req.RoomID, req.StartTime, req.EndTime))
        {
            return null;
        }

        var booking = new RoomBooking(req.RoomID, req.UserID, req.StartTime, req.EndTime);
        _roomBookingrepo.Add(booking);
        _roomBookingrepo.SaveChanges();
        return booking;
    }

    public bool DeleteBooking(int id)
    {
        var booking = _roomBookingrepo.GetById(id);
        if (booking is null) 
            return false;

        _roomBookingrepo.Delete(booking);
        return true;
    }

    public bool UpdateBooking(int id, UpdateRoomBookingRequest req)
    {
        var booking = _roomBookingrepo.GetById(id);
        if (booking is null) 
            return false;

        bool overlap = _roomBookingrepo
            .GetBy(b =>
                b.ID != id &&
                b.RoomID == req.RoomID &&
                b.StartTime < req.EndTime &&
                req.StartTime < b.EndTime
            )
            .Any();

        if (overlap)
            return false;

        booking.RoomID = req.RoomID;
        booking.UserID = req.UserID;
        booking.StartTime = req.StartTime;
        booking.EndTime = req.EndTime;
        _roomBookingrepo.SaveChanges();

        return true;
    }


}