using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using BusinessLayer.Interfaces;

public class EmailBL : IEmailBL
{
    private readonly IConfiguration _configuration;

    public EmailBL(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void SendEmail(string to, string subject, string token)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Greeting App", _configuration["EmailSettings:SenderEmail"]));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;

            // ✅ Ensure the email body contains only the token (Plain Text)
            message.Body = new TextPart("plain")
            {
                Text = $"Your password reset token is: {token}"
            };

            using (var client = new SmtpClient())
            {
                client.Connect(_configuration["EmailSettings:SmtpServer"], int.Parse(_configuration["EmailSettings:Port"]), MailKit.Security.SecureSocketOptions.StartTls);
                client.Authenticate(_configuration["EmailSettings:SenderEmail"], _configuration["EmailSettings:SenderPassword"]);
                client.Send(message);
                client.Disconnect(true);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Email send failed: {ex.Message}");
        }
    }

}
