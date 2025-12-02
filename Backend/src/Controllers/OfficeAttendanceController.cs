using Backend.Authorization;
using Backend.Dtos;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("officeattendance")]
public class OfficeAttendanceController(DatabaseContext db) : ControllerBase {
    private readonly DatabaseContext db = db;

    [HttpPut("")]

    public IActionResult CreateAttendance([FromBody] NewOfficeAttendanceRequest? attendanceRequest) {
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
    [HttpGet("")]

    public IActionResult CreateAttendance([FromQuery(Name = "date")] string dateString) {
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