using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;


namespace Yarışma.Services
{
    public class CustomEmailService:ICustomEmailService
    {
         public async Task SendEmailAsync(string to, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Proje Yarışması", "ecmnozkn@gmail.com"));
            email.To.Add(new MailboxAddress("", to));
            email.Subject = subject;
            email.Body = new TextPart("plain")
            {
                Text = body
            };

            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync("ecmnozkn@gmail.com", "iofh ocle qyeu sdtb");
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
        }
        public async Task SendResetPasswordEmail(string to, string resetLink)
        {
            string subject = "Şifre Sıfırlama Bağlantısı";
            string body = $"Şifre sıfırlama bağlantınız: {resetLink}";

            await SendEmailAsync(to, subject, body);
        }
    }
}
