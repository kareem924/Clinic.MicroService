using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Email
{
   public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);

        Task Execute(string apiKey, string subject, string message, string email);
    }
}
