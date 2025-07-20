namespace Infrastructure.Interfaces;

public interface IEmailService
{
    Task SendResetPasswordTokenToEmailAsync(string toEmail, string token);
}
