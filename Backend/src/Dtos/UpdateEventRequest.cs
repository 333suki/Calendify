using Backend.Models;

namespace Backend.Dtos;

public record UpdateEventRequest(Nullable<EventType> Type, string? Title, string? Description, DateTime? Date = null);