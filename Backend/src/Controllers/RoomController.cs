using Backend.Authorization;
using Backend.Dtos;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ExtendCookie(0, 0, 10, 0)]
[ApiController]
[Route("room")]
public class RoomController(DatabaseContext db) : ControllerBase {
    private readonly DatabaseContext db = db;

    [ServiceFilter(typeof(JwtAuthFilter))]
    [HttpGet("")]
    public IActionResult GetRooms()
    {
        return Ok(db.Rooms);
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

        Room? room = db.Rooms.FirstOrDefault(r => r.Name == newRoomRequest.Name);
        if(room is not null)
        {
            return Conflict(
                new
                {
                    message = "Room with that name already exists"
                }
            );
        }

        Room? newRoom = new Room(newRoomRequest.Name);

        db.Rooms.Add(newRoom);
        db.SaveChanges();
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

        Room? room = db.Rooms.Find(id);
        if (room is null)
            return NotFound(new { message = "Room not found" });

        db.Rooms.Remove(room);
        db.SaveChanges();

        return Ok(new { message = "Room deleted" });
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

        Room? room = db.Rooms.Find(id);
        if (room is null)
            return NotFound(
                new { 
                        message = "Room not found" 
                    }
                );

        room.Name = req.Name;
        db.SaveChanges();

        return Ok(
            new { 
                message = "Room updated" 
                }
            );
    }
}
