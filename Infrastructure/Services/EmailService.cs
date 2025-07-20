using System.Net;
using System.Net.Mail;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class EmailService(IConfiguration config) : IEmailService
{
    public async Task SendResetPasswordTokenToEmailAsync(string toEmail, string token)
    {
        var fromEmail = "romasafarov.ru@gmail.com";
        var fromPassword = config["EmailSettingPassword"];
        var subject = "Password reset email!";
        var body = $"Token for reseting your Password: /n{token}";

        var smtpClient = new SmtpClient
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            Credentials = new NetworkCredential(fromEmail, fromPassword)
        };

        using var messenge = new MailMessage(fromEmail, toEmail, subject, body);
        await smtpClient.SendMailAsync(messenge);
    }
}
