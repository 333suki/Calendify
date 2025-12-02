using Backend.Models;

namespace Backend.Dtos;

public record class UpdateOfficeAttendanceRequest(DateOnly? Date = null, OfficeAttendanceStatus? AttendanceStatus = null);
