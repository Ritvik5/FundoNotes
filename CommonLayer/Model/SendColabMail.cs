using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CommonLayer.Model
{
    public class SendColabMail
    {
        public void EmailService(string email)
        {
            try
            {
                string fromEmail = "ritvikbridgelabz@gmail.com";

                string body = $"{fromEmail} Add You as an Colabaretor with his Note";
                string subject = "Colabaretion Info";
                var smtp = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromEmail, "rgdircfcudbzxeil"),
                    EnableSsl = true,
                };
                using (var message = new MailMessage(fromEmail, email, subject, body))
                {
                    message.IsBodyHtml = true;
                    smtp.Send(message);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
