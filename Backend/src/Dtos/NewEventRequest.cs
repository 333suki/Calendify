using Backend.Models;

namespace Backend.Dtos;

public record class NewEventRequest(Nullable<EventType> Type, string? Title, string? Description, DateTime? Date = null);
