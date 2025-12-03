using Backend.Models;

namespace Backend.Dtos;

public record class NewOfficeAttendanceRequest(DateOnly? Date = null, OfficeAttendanceStatus? AttendanceStatus = null);
