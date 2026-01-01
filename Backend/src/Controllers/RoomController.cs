using Backend.Authorization;
using Backend.Dtos;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("room")]
public class RoomController : ControllerBase {
    private static readonly IRoomService _roomService;

    [HttpGet("")]
    public IActionResult GetRooms()
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
        var rooms = _roomService.GetRooms();
        return Ok(rooms);
    }

    [HttpPost("")]
    public IActionResult CreateRoom([FromBody] NewRoomRequest? newRoomRequest)
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
                            message = "Invalid Authorization header"
                        }
                    );
                case AuthUtils.TokenParseResult.Invalid:
                        return Unauthorized(
                        new
                        {
                            message = "No authorization header provided"
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

    [HttpDelete("{id}")]
    public IActionResult DeleteRoom(int id)
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
                            message = "Invalid Authorization header"
                        }
                    );
                case AuthUtils.TokenParseResult.Invalid:
                        return Unauthorized(
                        new
                        {
                            message = "No authorization header provided"
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



    [HttpPut("{id}")]
    public IActionResult UpdateRoom(int id, [FromBody] UpdateRoomRequest? req)
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
                            message = "Invalid Authorization header"
                        }
                    );
                case AuthUtils.TokenParseResult.Invalid:
                        return Unauthorized(
                        new
                        {
                            message = "No authorization header provided"
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