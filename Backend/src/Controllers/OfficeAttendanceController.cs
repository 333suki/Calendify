using Backend.Authorization;
using Backend.Dtos;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ExtendCookie(0, 0, 10, 0)]
[ApiController]
[Route("officeattendance")]
public class OfficeAttendanceController(DatabaseContext db) : ControllerBase {
    private readonly DatabaseContext db = db;

    [ServiceFilter(typeof(JwtAuthFilter))]
    [HttpPut("")]
    public IActionResult CreateAttendance([FromBody] NewOfficeAttendanceRequest? attendanceRequest) {
        var payload = HttpContext.Items["jwtPayload"] as Payload;
        
        if (attendanceRequest is null) {
            return BadRequest(
                new
                {
                    message = "Missing request body"
                }
            );
        }

        if (attendanceRequest.AttendanceStatus is null || attendanceRequest.Date is null)
        {
            return BadRequest(
                new
                {
                    message = "Missing request fields"
                }
            );
        }
        
        OfficeAttendance? officeAttendance = db.OfficeAttendances
            .FirstOrDefault(a => a.UserID == Convert.ToInt32(payload!.Sub) && a.Date == attendanceRequest.Date);
        
        if (officeAttendance is not null)
        {
            officeAttendance.Status = attendanceRequest.AttendanceStatus;
            db.SaveChanges();
            return Ok(
                new
                {
                    message = "Attendance updated"
                }
            );
        }

        db.OfficeAttendances.Add(new OfficeAttendance(Convert.ToInt32(payload!.Sub), attendanceRequest.Date, attendanceRequest.AttendanceStatus));
        db.SaveChanges();
        return Ok(
            new
            {
                message = "Attendance registered"
            }
        );
    }
    
    [ServiceFilter(typeof(JwtAuthFilter))]
    [HttpGet("")]
    public IActionResult GetAttendance([FromQuery(Name = "date")] string dateString) {
        var payload = HttpContext.Items["jwtPayload"] as Payload;
        DateOnly? date = DateOnly.Parse(dateString);
        OfficeAttendance? officeAttendance = db.OfficeAttendances
            .FirstOrDefault(a => a.Date == date && a.UserID == Convert.ToInt32(payload!.Sub));

        if (officeAttendance is null)
        {
            return NotFound(
                new
                {
                    message = "No attendance registered on this date"
                }
            );
        }
        
        return Ok(
            new
            {
                status = officeAttendance.Status
            }
        );
    }
}
