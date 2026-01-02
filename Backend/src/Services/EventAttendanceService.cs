using Backend.Dtos;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class EventAttendanceService : IEventAttendanceService
{
    private readonly IRepository<EventAttendance> _attendanceRepo;
    private readonly IRepository<Event> _eventRepo;


    public EventAttendanceService(IRepository<EventAttendance> attendanceRepo,IRepository<Event> eventRepo)
    {
        _attendanceRepo = attendanceRepo;
        _eventRepo = eventRepo;
    }

    public bool EventExists(int eventid)
    {
        Event? eventExists = _eventRepo.GetById(eventid);
        if (eventExists is null)
        {
            return false;
        }
        return true;
    }


    public bool AttendanceExists(int userId, int eventId)
    {
        return _attendanceRepo
            .GetAll()
            .Any(ea => ea.UserID == userId && ea.EventID == eventId);
    }

    public bool RegisterAttendance(int userId, NewEventAttendanceRequest request)
    {
        if (request.EventID is null)
        {
            return false;
        }

        // Event? eventExists = _eventRepo.GetById(request.EventID.Value);
        // if (eventExists is null)
        // {
        //     return false;
        // }

        EventAttendance? existingAttendance = _attendanceRepo
            .GetBy(ea => ea.UserID == userId && ea.EventID == request.EventID.Value)
            .FirstOrDefault();

        if (existingAttendance is not null)
        {
            return false;
        }

        _attendanceRepo.Add(
            new EventAttendance(userId, request.EventID.Value)
        );
        _attendanceRepo.SaveChanges();

        return true;
    }

    public IEnumerable<EventAttendance> GetUserAttendances(int userId)
    {
        return _attendanceRepo.GetBy(ea => ea.UserID == userId);
    }


    public IEnumerable<EventAttendance>? GetEventAttendances(int eventId)
    {
        Event? eventExists = _eventRepo.GetById(eventId);
        if (eventExists is null)
        {
            return null;
        }

        return _attendanceRepo.GetBy(ea => ea.EventID == eventId);
    }


    public bool UnregisterAttendance(int userId, int eventId)
    {
        EventAttendance? attendance = _attendanceRepo
            .GetBy(ea => ea.UserID == userId && ea.EventID == eventId)
            .FirstOrDefault();

        if (attendance is null)
        {
            return false;
        }

        _attendanceRepo.Delete(attendance);
        _attendanceRepo.SaveChanges();

        return true;
    }

}
