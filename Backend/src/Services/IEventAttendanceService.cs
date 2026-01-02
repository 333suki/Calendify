using Backend.Dtos;
using Backend.Models;

namespace Backend.Services;

public interface IEventAttendanceService
{
    bool EventExists(int eventId);
    bool AttendanceExists(int userId, int eventId);

    bool RegisterAttendance(int userId, NewEventAttendanceRequest request);

    IEnumerable<EventAttendance> GetUserAttendances(int userId);

    IEnumerable<EventAttendance>? GetEventAttendances(int eventId);

    bool UnregisterAttendance(int userId, int eventId);
}
