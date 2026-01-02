using Backend.Authorization;
using Backend.Dtos;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ExtendCookie(0, 0, 10, 0)]
[ApiController]
[Route("room")]
public class RoomController : ControllerBase {
    private static readonly IRoomService _roomService;

    [ServiceFilter(typeof(JwtAuthFilter))]
    [HttpGet("")]
    public IActionResult GetRooms()
    {
   
        var rooms = _roomService.GetRooms();
        return Ok(rooms);

    }

    [ServiceFilter(typeof(JwtAuthFilter))]
    [HttpPost("")]
    public IActionResult CreateRoom([FromBody] NewRoomRequest? newRoomRequest)
    {
        var payload = HttpContext.Items["jwtPayload"] as Payload;
        if(payload!.Role != (int)Role.Admin)
        {
            return Unauthorized(
                new
                {
                    message = "User is not admin"
                }
            );
            
        }

        if (newRoomRequest is null)
        {
            return BadRequest(
                new
                {
                    message = "Empty request body"
                }
            );
        }

        if (newRoomRequest.Name is null)
        {
            return BadRequest(
                new
                {
                    message = "Missing room fields"
                }
            );
        }

        bool room = _roomService.CreateRoom(newRoomRequest.Name);
        if(room is false)
        {
            return Conflict(
                new
                {
                    message = "Room with that name already exists"
                }
            );
        }

        return Ok(
            new 
            {
                message = "Room created succesfully"
            }
        );
    }

    [ServiceFilter(typeof(JwtAuthFilter))]
    [HttpDelete("{id}")]
    public IActionResult DeleteRoom(int id)
    {
        var payload = HttpContext.Items["jwtPayload"] as Payload;
        if (payload!.Role != (int)Role.Admin)
            return Unauthorized(
                new { 
                    message = "User is not admin" 
                    }
                );

        bool roomDeleted = _roomService.DeleteRoom(id);

        if (roomDeleted is false)
            return NotFound(
                new 
                { 
                    message = "Room not found" 
                }
            );


        return Ok(
                    new 
                    { 
                        message = "Room deleted" 
                    }
                );
    }
    
    [ServiceFilter(typeof(JwtAuthFilter))]
    [HttpPut("{id}")]
    public IActionResult UpdateRoom(int id, [FromBody] UpdateRoomRequest? req)
    {
        var payload = HttpContext.Items["jwtPayload"] as Payload;
        if (payload!.Role != (int)Role.Admin)
            return Unauthorized(
                new { 
                        message = "User is not admin" 
                    }
                );

        if (req is null)
        {
            return BadRequest(
                new
                {
                    message = "Empty request body"
                }
            );
        }

        if (req.Name is null)
        {
            return BadRequest(
                new
                {
                    message = "Missing room fields"
                }
            );
        }

        bool room = _roomService.UpdateRoom(id, req.Name);

        if (room is false)
            return NotFound(
                new { 
                        message = "Room not found" 
                    }
                );

        return Ok(
            new { 
                message = "Room updated" 
                }
            );
    }
}
