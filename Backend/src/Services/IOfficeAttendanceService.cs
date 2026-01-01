using System.Linq.Expressions;
using Backend.Dtos;
using Backend.Models;

namespace Backend.Services;

public interface IOfficeAttendanceService
{

  OfficeAttendance? GetAttendance(int UserID, string date);

  OfficeAttendance CreateAttendance(int userID, NewOfficeAttendanceRequest req);

}

