using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("rahafsalhab16@gmail.com", "mbsa czdy ddsy keqj\r\n")
            };

            return client.SendMailAsync(
                new MailMessage(from: "rahafsalhab16@gmail.com",
                                to: email,
                                subject,
                                htmlMessage
                                )
                         {IsBodyHtml=true});
        }
    }
}
