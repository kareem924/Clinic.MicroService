using System.Threading.Tasks;
using Common.Email;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Security.Infrastructure.Service
{
    public class EmailSenderService : IEmailSender
    {
        private readonly IOptions<AuthMessageSenderOptions> _optionsAccessor;

        public EmailSenderService(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            _optionsAccessor = optionsAccessor;
        }



        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(_optionsAccessor.Value.SendGridKey, subject, message, email);
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("kareemmuhammad924@gmail.com", "Kareem Muhammad"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}

