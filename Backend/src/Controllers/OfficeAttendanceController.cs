using Backend.Authorization;
using Backend.Dtos;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("officeattendance")]
public class OfficeAttendanceController : ControllerBase {
    private readonly IOfficeAttendanceService _officeAttendanceService;

    public OfficeAttendanceController(IOfficeAttendanceService officeAttendanceService) {
        _officeAttendanceService = officeAttendanceService;
    }

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

        OfficeAttendance officeAttendance = _officeAttendanceService.CreateAttendance(Convert.ToInt32(payload!.Sub), attendanceRequest);

        if(officeAttendance is null)
        {
            return BadRequest(

                new
                {
                    message = "Attendance not created or does not exist"
                }
            );
 
        }

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

        OfficeAttendance? officeAttendance = _officeAttendanceService.GetAttendance(Convert.ToInt32(payload!.Sub), dateString);

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
