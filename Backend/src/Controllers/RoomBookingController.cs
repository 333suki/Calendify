using Backend.Authorization;
using Backend.Dtos;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("room-bookings")]

public class RoomBookingController(DatabaseContext db) : ControllerBase
{
    private readonly DatabaseContext db = db;

    [HttpPost("")]
    public IActionResult CreateBooking([FromBody] NewRoomBookingRequest? req)
    {
        var request = Request;
        if (!request.Headers.TryGetValue("Authorization", out var authHeader)) {
            return Unauthorized(
                new
                {
                    message = "No authorization header provided"
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

        if (req is null)
        {
            return BadRequest(
                new
                {
                    message = "Empty request body"
                }
            );
        }

        if (req.UserID is null || req.RoomID is null || req.StartTime is null || req.EndTime is null)
        {
            return BadRequest(
                new
                {
                    message = "Missing room fields"
                }
            );
        }

        Room? room = db.Rooms.Find(req.RoomID);
        if (room is null) {
            return NotFound(
                new {
                    message = "Room not found" 
                }
            );
        }

        bool overlap = db.RoomBookings.Any(b =>
            b.RoomID == req.RoomID &&
            b.StartTime < req.EndTime &&
            req.StartTime < b.EndTime
        );

        if (overlap) {
            return Conflict(
                new {
                    message = "Room already booked in that time period" 
                }
            );
        }

        RoomBooking booking = new RoomBooking(
            req.RoomID,
            req.UserID,
            req.StartTime,
            req.EndTime
        );

        db.RoomBookings.Add(booking);
        db.SaveChanges();

        return Ok(
            new { 
                    message = "Booking created" 
                }
            );
    }


    [HttpGet("")]
    public IActionResult GetBookings()
    {
        var request = Request;
        if (!request.Headers.TryGetValue("Authorization", out var authHeader)) {
            return Unauthorized(
                new
                {
                    message = "No authorization header provided"
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

        if (payload!.Role == (int)Role.Admin) {
            return Ok(db.RoomBookings.ToList());
        }

        return Ok(db.RoomBookings.Where(rb => rb.UserID == Convert.ToInt32(payload.Sub)).ToList());
    }


    [HttpDelete("{id}")]
    public IActionResult DeleteBooking(int id)
    {
        var request = Request;
        if (!request.Headers.TryGetValue("Authorization", out var authHeader)) {
            return Unauthorized(
                new
                {
                    message = "No authorization header provided"
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
            return Unauthorized(
                new {
                        message = "User is not admin" 
                    }
                );

        RoomBooking? booking = db.RoomBookings.Find(id);
        if (booking is null)
            return NotFound(
                new { 
                    message = "Booking not found" 
                    }
                );

        db.RoomBookings.Remove(booking);
        db.SaveChanges();

        return Ok(new { message = "Booking deleted" });
    }

    [HttpPut("{id}")]
    public IActionResult UpdateBooking(int id, [FromBody] UpdateRoomBookingRequest req)
    {
        var request = Request;
        if (!request.Headers.TryGetValue("Authorization", out var authHeader)) {
            return Unauthorized(
                new
                {
                    message = "No authorization header provided"
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

        if (payload!.Role != (int)Role.Admin) {
            return Unauthorized(
                new { 
                    message = "User is not admin" 
                }
            );
        }
        
        RoomBooking? booking = db.RoomBookings.Find(id);
        if (booking is null)
            return NotFound(new { message = "Booking not found" });

        Room? room = db.Rooms.Find(req.roomID);
        if (room is null)
            return NotFound(new { message = "Room not found" });

        User? user = db.Users.Find(req.userID);
        if (user is null)
            return NotFound(new { message = "User not found" });

        bool overlap = db.RoomBookings.Any(b =>
            b.ID != id &&  
            b.RoomID == req.roomID &&
            b.StartTime < req.endtime &&
            req.starttime < b.EndTime
        );

        if (overlap)
            return Conflict(
                new {
                     message = "Room already booked in that time period" 
                     }
                    );

        booking.RoomID = req.roomID;
        booking.UserID = req.userID;
        booking.StartTime = req.starttime;
        booking.EndTime = req.endtime;

        db.SaveChanges();

        return Ok(new { message = "Booking updated" });
    }
}
