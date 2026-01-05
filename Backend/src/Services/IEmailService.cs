using Backend.Models;

namespace Backend.Services;

public interface IEmailService
{
    Task SendPasswordResetEmailAsync(User user, string resetToken);
}
