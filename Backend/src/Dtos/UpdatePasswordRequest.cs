namespace Backend.Dtos;

public record class UpdatePasswordRequest(string CurrentPassword, string NewPassword);
