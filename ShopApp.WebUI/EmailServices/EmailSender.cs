using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ShopApp.WebUI.EmailServices
{
    public class EmailSender:IEmailSender
    {
        
        private const string SendGridKey = "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";
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //email dışardan gelen mail
            return Execute(SendGridKey, subject, htmlMessage, email);
        }

        private Task Execute(string sendGridKey, string subject, string message, string email)
        {
            //kullanmak için dotnet add package SendGrid kur
            var client = new SendGridClient(sendGridKey);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress("info@shopapp.com", "Shop App Deniz"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            //paramtre olarak gelen kişiye gidecek
            //msg.AddBcc("turkmen_deniz@hotmail.com"); // kopyasının gitmesi
            msg.AddTo(new EmailAddress(email));
            return client.SendEmailAsync(msg);
        }
    }
}
