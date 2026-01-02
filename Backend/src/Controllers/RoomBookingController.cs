using Backend.Authorization;
using Backend.Dtos;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("room-bookings")]

public class RoomBookingController : ControllerBase
{
    private readonly IRoomBookingService _roomBookingService;

    public RoomBookingController(IRoomBookingService roomBookingService) {
        _roomBookingService = roomBookingService;
    }

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

        bool room = _roomBookingService.RoomExists(req.RoomID);
        if (room is false) {
            return NotFound(
                new {
                    message = "Room not found" 
                }
            );
        }

        RoomBooking booking = _roomBookingService.CreateBooking(req);

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
            return Ok(_roomBookingService.GetBooking(Convert.ToInt32(payload.Sub), true).ToList());
        }

        return Ok(_roomBookingService.GetBooking(Convert.ToInt32(payload.Sub), false).ToList());
    }

    [ServiceFilter(typeof(JwtAuthFilter))]
    [HttpDelete("{id}")]
    public IActionResult DeleteBooking(int id)
    {
      var payload = HttpContext.Items["jwtPayload"] as Payload;

        if (payload!.Role != (int)Role.Admin)
            return Unauthorized(
                new {
                        message = "User is not admin" 
                    }
                );

        bool? booking = _roomBookingService.DeleteBooking(id);        
        
 
        if (booking is null)
            return NotFound(
                new { 
                    message = "Booking not found" 
                    }
                );
        
        if (payload!.Role != (int)Role.Admin && payload.Sub != id.ToString()) {
            return Unauthorized(
                new {
                    message = "User can only delete their own booking"
                }
            );
        }

        return Ok(
            new 
            {
                 message = "Booking deleted" 
            }
            );
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
        
        
        bool updatedBooking = _roomBookingService.UpdateBooking(id, req);


        if (updatedBooking is false)
        {
            return BadRequest(
                new { 
                    message = "Booking not updated" 
                    }
                ); 
        }

        return Ok(
                new { 
                    message = "Booking updated" 
                    }
                );
    }
}
