using Backend.Dtos;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class OfficeAttendanceService : IOfficeAttendanceService
{
    private readonly IRepository<OfficeAttendance> _attendanceRepo;


    public OfficeAttendanceService(IRepository<OfficeAttendance> attendanceRepo)
    {
        _attendanceRepo = attendanceRepo;
    }

    public OfficeAttendance CreateAttendance(int userID, NewOfficeAttendanceRequest req)
    {
        var existingAttendance = _attendanceRepo.GetBy(a => a.UserID == userID && a.Date == req.Date).FirstOrDefault();

        if (existingAttendance is not null)
        {
            existingAttendance.Status = req.AttendanceStatus!;
            Console.WriteLine("Updating");
            Console.WriteLine(existingAttendance.Status);
            _attendanceRepo.Update(existingAttendance);
            _attendanceRepo.SaveChanges();
            return existingAttendance;
        }

        var newAttendance = new OfficeAttendance(userID, req.Date!, req.AttendanceStatus!);

        Console.WriteLine("Creating");
        _attendanceRepo.Add(newAttendance);
        _attendanceRepo.SaveChanges();
        return newAttendance;
    }

    public OfficeAttendance? GetAttendance(int userId, string dateString)
    {
        DateOnly? date = DateOnly.Parse(dateString);

        OfficeAttendance? attendance = _attendanceRepo.GetBy(a => a.UserID == userId && a.Date == date).FirstOrDefault();

        return attendance;
    }
}
