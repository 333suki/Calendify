using Backend.Authorization;
using Backend.Dtos;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ExtendCookie(0, 0, 10, 0)]
[ApiController]
[Route("room-bookings")]
public class RoomBookingController(DatabaseContext db) : ControllerBase
{
    private readonly DatabaseContext db = db;

    [ServiceFilter(typeof(JwtAuthFilter))]
    [HttpPost("")]
    public IActionResult CreateBooking([FromBody] NewRoomBookingRequest? req)
    {
        var payload = HttpContext.Items["jwtPayload"] as Payload;
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
        
        if (payload!.Role != (int)Role.Admin && payload.Sub != req.UserID.ToString()) {
            return Unauthorized(
                new {
                    message = "User can only create a booking for themselves"
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

    [ServiceFilter(typeof(JwtAuthFilter))]
    [HttpGet("")]
    public IActionResult GetBookings()
    {
        var payload = HttpContext.Items["jwtPayload"] as Payload;
        if (payload!.Role == (int)Role.Admin) {
            return Ok(db.RoomBookings.ToList());
        }

        return Ok(db.RoomBookings.Where(rb => rb.UserID == Convert.ToInt32(payload.Sub)).ToList());
    }

    [ServiceFilter(typeof(JwtAuthFilter))]
    [HttpDelete("{id}")]
    public IActionResult DeleteBooking(int id)
    {
        var payload = HttpContext.Items["jwtPayload"] as Payload;
        RoomBooking? booking = db.RoomBookings.Find(id);
        if (booking is null)
            return NotFound(
                new { 
                    message = "Booking not found" 
                    }
                );
        
        if (payload!.Role != (int)Role.Admin && payload.Sub != booking.UserID.ToString()) {
            return Unauthorized(
                new {
                    message = "User can only delete their own booking"
                }
            );
        }

        db.RoomBookings.Remove(booking);
        db.SaveChanges();

        return Ok(new { message = "Booking deleted" });
    }
    
    [ServiceFilter(typeof(JwtAuthFilter))]
    [HttpPut("{id}")]
    public IActionResult UpdateBooking(int id, [FromBody] UpdateRoomBookingRequest? req)
    {
        var payload = HttpContext.Items["jwtPayload"] as Payload;
        if (req is null)
        {
            return BadRequest(
                new
                {
                    message = "Empty request body"
                }
            );
        }
        
        if (payload!.Role != (int)Role.Admin && payload.Sub != req.UserID.ToString()) {
            return Unauthorized(
                new {
                    message = "User can only update a booking for themselves"
                }
            );
        }
        
        RoomBooking? booking = db.RoomBookings.Find(id);
        if (booking is null)
            return NotFound(new { message = "Booking not found" });

        Room? room = db.Rooms.Find(req.RoomID);
        if (room is null)
            return NotFound(new { message = "Room not found" });

        User? user = db.Users.Find(req.UserID);
        if (user is null)
            return NotFound(new { message = "User not found" });

        bool overlap = db.RoomBookings.Any(b =>
            b.ID != id &&  
            b.RoomID == req.RoomID &&
            b.StartTime < req.EndTime &&
            req.StartTime < b.EndTime
        );

        if (overlap)
            return Conflict(
                new {
                     message = "Room already booked in that time period" 
                     }
                    );

        booking.RoomID = req.RoomID;
        booking.UserID = req.UserID;
        booking.StartTime = req.StartTime;
        booking.EndTime = req.EndTime;

        db.SaveChanges();

        return Ok(new { message = "Booking updated" });
    }
}
