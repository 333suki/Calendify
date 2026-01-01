using Backend.Authorization;
using Backend.Dtos;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("eventattendance")]
public class EventAttendanceController : ControllerBase {
    private readonly IEventAttendanceService? _eventAttendanceService;

    [HttpPut("")]
    public IActionResult RegisterAttendance([FromBody] NewEventAttendanceRequest? attendanceRequest)
    {
        var request = Request;
        if (!request.Headers.TryGetValue("Authorization", out var authHeader)) {
            return Unauthorized(
                new
                {
                    message = "No Authorization header provided"
                }
            );
        }
        
        if (!AuthUtils.ParseToken(authHeader.ToString(), out AuthUtils.TokenParseResult result, out Header? header, out Payload? payload)) {
            switch (result) {
                case AuthUtils.TokenParseResult.InvalidFormat:
                    return BadRequest(
                        new
                        {
                            message = "Invalid Authorization token format"
                        }
                    );
                case AuthUtils.TokenParseResult.Invalid:
                    return Unauthorized(
                        new
                        {
                            message = "Invalid Authorization token"
                        }
                    );
                case AuthUtils.TokenParseResult.TokenExpired:
                    return StatusCode(498,
                        new
                        {
                            message = "Token expired"
                        }
                    );
                case AuthUtils.TokenParseResult.HeaderNullOrEmpty:
                case AuthUtils.TokenParseResult.PayloadNullOrEmpty:
                case AuthUtils.TokenParseResult.SignatureNullOrEmpty:
                    return BadRequest(
                        new
                        {
                            message = "Invalid Authorization header"
                        }
                    );
                case AuthUtils.TokenParseResult.HeaderDeserializeError:
                    return StatusCode(500,
                        new
                        {
                            message = "Header deserialization error"
                        }
                    );
                case AuthUtils.TokenParseResult.PayloadDeserializeError:
                    return StatusCode(500,
                        new
                        {
                            message = "Payload deserialization error"
                        }
                    );
            }
        }

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

    [HttpGet("")]
    public IActionResult GetUserAttendances()
    {
        var request = Request;
        if (!request.Headers.TryGetValue("Authorization", out var authHeader)) {
            return Unauthorized(
                new
                {
                    message = "No Authorization header provided"
                }
            );
        }
        
        if (!AuthUtils.ParseToken(authHeader.ToString(), out AuthUtils.TokenParseResult result, out Header? header, out Payload? payload)) {
            switch (result) {
                case AuthUtils.TokenParseResult.InvalidFormat:
                    return BadRequest(
                        new
                        {
                            message = "Invalid Authorization token format"
                        }
                    );
                case AuthUtils.TokenParseResult.Invalid:
                    return Unauthorized(
                        new
                        {
                            message = "Invalid Authorization token"
                        }
                    );
                case AuthUtils.TokenParseResult.TokenExpired:
                    return StatusCode(498,
                        new
                        {
                            message = "Token expired"
                        }
                    );
                case AuthUtils.TokenParseResult.HeaderNullOrEmpty:
                case AuthUtils.TokenParseResult.PayloadNullOrEmpty:
                case AuthUtils.TokenParseResult.SignatureNullOrEmpty:
                    return BadRequest(
                        new
                        {
                            message = "Invalid Authorization header"
                        }
                    );
                case AuthUtils.TokenParseResult.HeaderDeserializeError:
                    return StatusCode(500,
                        new
                        {
                            message = "Header deserialization error"
                        }
                    );
                case AuthUtils.TokenParseResult.PayloadDeserializeError:
                    return StatusCode(500,
                        new
                        {
                            message = "Payload deserialization error"
                        }
                    );
            }
        }

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

    [HttpGet("{eventID:int}")]
    public IActionResult GetEventAttendances(int eventID)
    {
        var request = Request;
        if (!request.Headers.TryGetValue("Authorization", out var authHeader)) {
            return Unauthorized(
                new
                {
                    message = "No Authorization header provided"
                }
            );
        }
        
        if (!AuthUtils.ParseToken(authHeader.ToString(), out AuthUtils.TokenParseResult result, out Header? header, out Payload? payload)) {
            switch (result) {
                case AuthUtils.TokenParseResult.InvalidFormat:
                    return BadRequest(
                        new
                        {
                            message = "Invalid Authorization token format"
                        }
                    );
                case AuthUtils.TokenParseResult.Invalid:
                    return Unauthorized(
                        new
                        {
                            message = "Invalid Authorization token"
                        }
                    );
                case AuthUtils.TokenParseResult.TokenExpired:
                    return StatusCode(498,
                        new
                        {
                            message = "Token expired"
                        }
                    );
                case AuthUtils.TokenParseResult.HeaderNullOrEmpty:
                case AuthUtils.TokenParseResult.PayloadNullOrEmpty:
                case AuthUtils.TokenParseResult.SignatureNullOrEmpty:
                    return BadRequest(
                        new
                        {
                            message = "Invalid Authorization header"
                        }
                    );
                case AuthUtils.TokenParseResult.HeaderDeserializeError:
                    return StatusCode(500,
                        new
                        {
                            message = "Header deserialization error"
                        }
                    );
                case AuthUtils.TokenParseResult.PayloadDeserializeError:
                    return StatusCode(500,
                        new
                        {
                            message = "Payload deserialization error"
                        }
                    );
            }
        }

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

    [HttpDelete("{eventID:int}")]
    public IActionResult UnregisterAttendance(int eventID)
    {
        var request = Request;
        if (!request.Headers.TryGetValue("Authorization", out var authHeader)) {
            return Unauthorized(
                new
                {
                    message = "No Authorization header provided"
                }
            );
        }
        
        if (!AuthUtils.ParseToken(authHeader.ToString(), out AuthUtils.TokenParseResult result, out Header? header, out Payload? payload)) {
            switch (result) {
                case AuthUtils.TokenParseResult.InvalidFormat:
                    return BadRequest(
                        new
                        {
                            message = "Invalid Authorization token format"
                        }
                    );
                case AuthUtils.TokenParseResult.Invalid:
                    return Unauthorized(
                        new
                        {
                            message = "Invalid Authorization token"
                        }
                    );
                case AuthUtils.TokenParseResult.TokenExpired:
                    return StatusCode(498,
                        new
                        {
                            message = "Token expired"
                        }
                    );
                case AuthUtils.TokenParseResult.HeaderNullOrEmpty:
                case AuthUtils.TokenParseResult.PayloadNullOrEmpty:
                case AuthUtils.TokenParseResult.SignatureNullOrEmpty:
                    return BadRequest(
                        new
                        {
                            message = "Invalid Authorization header"
                        }
                    );
                case AuthUtils.TokenParseResult.HeaderDeserializeError:
                    return StatusCode(500,
                        new
                        {
                            message = "Header deserialization error"
                        }
                    );
                case AuthUtils.TokenParseResult.PayloadDeserializeError:
                    return StatusCode(500,
                        new
                        {
                            message = "Payload deserialization error"
                        }
                    );
            }
        }

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