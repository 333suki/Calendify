using Backend.Models;

namespace Backend.Dtos;

public record UpdateEventRequest(string? Title, string? Description, DateTime? Date = null);