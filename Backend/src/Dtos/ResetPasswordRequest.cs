namespace Backend.Dtos;

public record class ResetPasswordRequest(string? Token, string? NewPassword);
