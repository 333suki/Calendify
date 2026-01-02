using Backend.Authorization;
using Backend.Dtos;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("event")]
public class EventController : ControllerBase {
    private static readonly IEventService _eventService;

    [HttpPost("")]
    public IActionResult CreateEvent([FromBody] NewEventRequest? newEventRequest)
    {
        var payload = HttpContext.Items["jwtPayload"] as Payload;
        if (payload!.Role != (int)Role.Admin)
        {
            return Unauthorized(
                new
                {
                    message = "User is not admin"
                }
            );
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

        if (newEventRequest.Type is null || newEventRequest.Title is null || newEventRequest.Description is null || newEventRequest.Date is null)
        {
            return BadRequest(
                new
                {
                    message = "Missing event fields"
                }
            );
        }

        _eventService.Create(newEventRequest);


        return Ok(
            new
            {
                message = "Event created successfully"
            }
        );
    }
    
    [ServiceFilter(typeof(JwtAuthFilter))]
    [HttpDelete("{id}")]
    public IActionResult DeleteEvent(int id)
    {
        var payload = HttpContext.Items["jwtPayload"] as Payload;
        if (payload!.Role != (int)Role.Admin)
        {
            return Unauthorized(
                new
                {
                    message = "User is not admin"
                }
            );
        }

        bool deleted = _eventService.Delete(id);

        if (!deleted)
        {
            return NotFound(new
            {
                message = "Event not found"
            });
        }

        return Ok(
            new
            {
                message = "Event deleted"
            }
        );
    }

    [ServiceFilter(typeof(JwtAuthFilter))]
    [HttpPut("{id}")]
    public IActionResult UpdateEvent(int id, [FromBody] UpdateEventRequest? updateEventRequest)
    {
        var payload = HttpContext.Items["jwtPayload"] as Payload;
        if (payload!.Role != (int)Role.Admin)
        {
            return Unauthorized(
                new
                {
                    message = "User is not admin"
                }
            );
        }

        if (updateEventRequest is null)
        {
            return BadRequest(
                new
                {
                    message = "Empty request body"
                }
            );
        }

        Event? updated = _eventService.Update(id, updateEventRequest);

        if (updated is null)

        {
            return NotFound(new
            {
                message = "Event not found"
            });
        }

        return Ok(new
        {
            message = "Event updated"
        });

        
    }

    [ServiceFilter(typeof(JwtAuthFilter))]
    [HttpGet("")]
    public IActionResult GetEventByDay([FromQuery(Name = "date")] string dateString)
    {
        var payload = HttpContext.Items["jwtPayload"] as Payload;
        DateOnly date = DateOnly.Parse(dateString);

        var Events = _eventService.GetByDay(date);

        if(Events is null)
        {
            return NotFound(new
            {
                message = "Event(s) not found"
            });
        }
        
        return Ok(
            Events
        );
    }
}
