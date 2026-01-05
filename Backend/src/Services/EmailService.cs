using Backend.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Backend.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendPasswordResetEmailAsync(User user, string resetToken)
    {
        var emailSettings = _configuration.GetSection("EmailSettings");

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Calendify", emailSettings["From"]));
        message.To.Add(new MailboxAddress(user.Username, user.Email));
        message.Subject = "Password Reset Request";

        var bodyBuilder = new BodyBuilder();
        bodyBuilder.HtmlBody = $@"
            <h2>Password Reset Request</h2>
            <p>Hello {user.Username},</p>
            <p>You have requested to reset your password. Please use the following token to reset your password:</p>
            <p><strong>Reset Token: {resetToken}</strong></p>
            <p>This token will expire in 1 hour.</p>
            <p>If you did not request this password reset, please ignore this email.</p>
            <br>
            <p>Best regards,<br>Calendify Team</p>
        ";

        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(emailSettings["SmtpServer"], int.Parse(emailSettings["Port"]), SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(emailSettings["Username"], emailSettings["Password"]);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
