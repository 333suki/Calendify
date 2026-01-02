using Backend.Authorization;
using Backend.Dtos;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ExtendCookie(0, 0, 10, 0)]
[ApiController]
[Route("officeattendance")]
public class OfficeAttendanceController : ControllerBase {
    private static readonly IOfficeAttendanceService _officeAttendanceService;

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


    [HttpGet("")]
    public IActionResult GetAttendance([FromQuery(Name = "date")] string dateString) {
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
