using Backend.Authorization;
using Backend.Dtos;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ExtendCookie(0, 0, 10, 0)]
[ApiController]
[Route("eventattendance")]
public class EventAttendanceController(DatabaseContext db) : ControllerBase {
    private readonly DatabaseContext db = db;

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

        Event? eventExists = db.Events.Find(attendanceRequest.EventID);
        if (eventExists is null)
        {
            return NotFound(
                new
                {
                    message = "Event not found"
                }
            );
        }

        EventAttendance? existingAttendance = db.EventAttendances
            .FirstOrDefault(ea => ea.UserID == userID && ea.EventID == attendanceRequest.EventID);

        if (existingAttendance is not null)
        {
            return Conflict(
                new
                {
                    message = "Already registered for this event"
                }
            );
        }

        db.EventAttendances.Add(new EventAttendance(userID, attendanceRequest.EventID.Value));
        db.SaveChanges();

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

        var attendances = db.EventAttendances
            .Where(ea => ea.UserID == userID)
            .Include(ea => ea.Event)
            .Select(ea => new {
                eventID = ea.EventID,
                title = ea.Event.Title,
                description = ea.Event.Description,
                date = ea.Event.Date
            })
            .ToList();

        return Ok(attendances);
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

        Event? eventExists = db.Events.Find(eventID);
        if (eventExists is null)
        {
            return NotFound(
                new
                {
                    message = "Event not found"
                }
            );
        }

        var attendances = db.EventAttendances
            .Where(ea => ea.EventID == eventID)
            .Include(ea => ea.User)
            .Select(ea => new {
                userID = ea.UserID,
                username = ea.User.Username,
                email = ea.User.Email
            })
            .ToList();

        return Ok(attendances);
    }

    [ServiceFilter(typeof(JwtAuthFilter))]
    [HttpDelete("{eventID:int}")]
    public IActionResult UnregisterAttendance(int eventID)
    {
        var payload = HttpContext.Items["jwtPayload"] as Payload;
        int userID = Convert.ToInt32(payload!.Sub);

        EventAttendance? attendance = db.EventAttendances
            .FirstOrDefault(ea => ea.UserID == userID && ea.EventID == eventID);

        if (attendance is null)
        {
            return NotFound(
                new
                {
                    message = "Attendance registration not found"
                }
            );
        }

        db.EventAttendances.Remove(attendance);
        db.SaveChanges();

        return Ok(
            new
            {
                message = "Event attendance unregistered"
            }
        );
    }
}