namespace Yarışma.Services
{
    public interface ICustomEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendResetPasswordEmail(string to, string resetLink);
    }
}
