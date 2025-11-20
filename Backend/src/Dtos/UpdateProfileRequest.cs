using Backend.Models;

namespace Backend.Dtos;

public record class UpdateProfileRequest(string? Username, string? Password, Role? Role);
