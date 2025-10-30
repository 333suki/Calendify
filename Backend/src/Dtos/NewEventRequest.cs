namespace Backend.Dtos;

public record class NewEventRequest(string? Title, string? Description, DateTime? Date = null);
