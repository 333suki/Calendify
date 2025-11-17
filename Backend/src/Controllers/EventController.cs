using System.Xml.Serialization;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Backend;
using Backend.Authorization;
using System.Text.Json;

namespace Backend.Controllers;

[ApiController]
[Route("event")]
public class EventController(DatabaseContext db) : ControllerBase {

    private readonly DatabaseContext db = db;
    
    [HttpPost("")]
    public IActionResult CreateEvent([FromBody] Backend.Dtos.NewEventRequest? newEventRequest, HttpRequest request)
    {
        if (!request.Headers.TryGetValue("Authorization", out var authHeader)) {
            return Unauthorized();
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
                    return Unauthorized();
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

        if (newEventRequest is null)
        {
            return BadRequest(
                new
                {
                    message = "Empty request body"
                }
            );
        }

        if (newEventRequest.Title is null || newEventRequest.Description is null || newEventRequest.Date is null)
        {
            return BadRequest(
                new
                {
                    message = "Missing event fields"
                }
            );
        }

        db.Events.Add(new Backend.Models.Event(newEventRequest.Title, newEventRequest.Description, newEventRequest.Date));
        db.SaveChanges();

        return Ok();
    }
}