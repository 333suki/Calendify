using Backend.Authorization;
using Backend.Dtos;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ExtendCookie(0, 0, 10, 0)]
[ApiController]
[Route("event")]
public class EventController(DatabaseContext db) : ControllerBase {
    private readonly DatabaseContext db = db;
    
    [ServiceFilter(typeof(JwtAuthFilter))]
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

        if (newEventRequest.Title is null || newEventRequest.Description is null || newEventRequest.Date is null)
        {
            return BadRequest(
                new
                {
                    message = "Missing event fields"
                }
            );
        }

        db.Events.Add(new Event(newEventRequest.Title, newEventRequest.Description, newEventRequest.Date));
        db.SaveChanges();

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

        Event? eventToDelete = db.Events.Find(id);
        if (eventToDelete is null)
        {
            return NotFound(
                new
                {
                    message = "Event not found"
                }
            );
        }

        db.Events.Remove(eventToDelete);
        db.SaveChanges();

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

        Event? eventToUpdate = db.Events.Find(id);
        if (eventToUpdate is null)
        {
            return NotFound(
                new
                {
                    message = "Event not found"
                }
            );
        }


        if (updateEventRequest.Title is not null)
        {
            eventToUpdate.Title = updateEventRequest.Title;
        }
        if (updateEventRequest.Description is not null)
        {
            eventToUpdate.Description = updateEventRequest.Description;
        }
        if (updateEventRequest.Date is not null)
        {
            eventToUpdate.Date = updateEventRequest.Date;
        }

        db.SaveChanges();

        return Ok(
            new
            {
                message = "Event updated"
            }
        );
    }

    [ServiceFilter(typeof(JwtAuthFilter))]
    [HttpGet("")]
    public IActionResult GetEventByDay([FromQuery(Name = "date")] string dateString)
    {
        var payload = HttpContext.Items["jwtPayload"] as Payload;
        DateOnly? date = DateOnly.Parse(dateString);
        var Events = db.Events
        .Where(e => DateOnly.FromDateTime(e.Date?? DateTime.Now) == date);
        return Ok(
            Events
        );
    }
}
