using Backend.Authorization;
using Backend.Dtos;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ExtendCookie(0, 0, 10, 0)]
[ApiController]
[Route("eventattendance")]
public class EventAttendanceController : ControllerBase {
    private readonly IEventAttendanceService? _eventAttendanceService;

    [ServiceFilter(typeof(JwtAuthFilter))]
    [HttpPut("")]
    public IActionResult RegisterAttendance([FromBody] NewEventAttendanceRequest? attendanceRequest)
    {
        if (attendanceRequest is null)
        {
            return BadRequest(
                new
                {
                    message = "Missing request body"
                }
            );
        }

        if (attendanceRequest.EventID is null)
        {
            return BadRequest(
                new
                {
                    message = "Missing request fields"
                }
            );
        }

        var payload = HttpContext.Items["jwtPayload"] as Payload;
        int userID = Convert.ToInt32(payload!.Sub);

        bool eventExists = _eventAttendanceService.EventExists(attendanceRequest.EventID.Value);
        if (eventExists is false)
        {
            return NotFound(
                new
                {
                    message = "Event not found"
                }
            );
        }

        bool alreadyRegistered = _eventAttendanceService.AttendanceExists(userID, attendanceRequest.EventID.Value);

        if (alreadyRegistered)
        {
            return Conflict(
                new
                { 
                    message = "Already registered for this event" 
                }
            );
        }

        _eventAttendanceService.RegisterAttendance(userID, attendanceRequest);

        return Ok(
            new
            {
                message = "Event attendance registered"
            }
        );
    }

    [ServiceFilter(typeof(JwtAuthFilter))]
    [HttpGet("")]
    public IActionResult GetUserAttendances()
    {
        var payload = HttpContext.Items["jwtPayload"] as Payload;
        int userID = Convert.ToInt32(payload!.Sub);

        var attendances = _eventAttendanceService.GetUserAttendances(userID);

        return Ok(attendances);

        // int userID = Convert.ToInt32(payload!.Sub);

        // var attendances = db.EventAttendances
        //     .Where(ea => ea.UserID == userID)
        //     .Include(ea => ea.Event)
        //     .Select(ea => new {
        //         eventID = ea.EventID,
        //         title = ea.Event.Title,
        //         description = ea.Event.Description,
        //         date = ea.Event.Date
        //     })
        //     .ToList();

        // return Ok(attendances);
    }

    [ServiceFilter(typeof(JwtAuthFilter))]
    [HttpGet("{eventID:int}")]
    public IActionResult GetEventAttendances(int eventID)
    {
        var payload = HttpContext.Items["jwtPayload"] as Payload;
        if (payload!.Role != (int)Role.Admin)
        {
            return Unauthorized(
                new
                {
                    message = "User is not admin"
                }
            );
        }

        bool? eventExists = _eventAttendanceService.EventExists(eventID);
        if (eventExists is false)
        {
            return NotFound(
                new
                {
                    message = "Event not found"
                }
            );
        }

        var attendances =  _eventAttendanceService.GetEventAttendances(eventID);

        // var attendances = db.EventAttendances
        //     .Where(ea => ea.EventID == eventID)
        //     .Include(ea => ea.User)
        //     .Select(ea => new {
        //         userID = ea.UserID,
        //         username = ea.User.Username,
        //         email = ea.User.Email
        //     })
        //     .ToList();

        return Ok(attendances);
    }

    [ServiceFilter(typeof(JwtAuthFilter))]
    [HttpDelete("{eventID:int}")]
    public IActionResult UnregisterAttendance(int eventID)
    {
        var payload = HttpContext.Items["jwtPayload"] as Payload;
        int userID = Convert.ToInt32(payload!.Sub);

        bool? attendance = _eventAttendanceService.AttendanceExists(userID, eventID);
        if (attendance is false)
        {
            return NotFound(
                new
                {
                    message = "Attendance registration not found"
                }
            );
        }
        _eventAttendanceService.UnregisterAttendance(userID, eventID);

        return Ok(
            new
            {
                message = "Event attendance unregistered"
            }
        );
    }
}