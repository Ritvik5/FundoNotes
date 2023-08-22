using Experimental.System.Messaging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CommonLayer.Model
{
    public class MsmqModel
    {
        MessageQueue fundoQueue = new MessageQueue();

        public void sendData2Queue(string token)
        {
            fundoQueue.Path = @".\Private$\Token";
            if(!MessageQueue.Exists(fundoQueue.Path))
            {
                MessageQueue.Create(fundoQueue.Path);
            }
            fundoQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            fundoQueue.ReceiveCompleted += FundoQueue_ReceiveCompleted;
            fundoQueue.Send(token);
            fundoQueue.BeginReceive();
            fundoQueue.Close();
        }

        private void FundoQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            var msg = fundoQueue.EndReceive(e.AsyncResult);
            string token = msg.Body.ToString();
            string body = $"<a style = \"color:#00802b; text-decoration: none; font-size:20px;\" href='http://localhost:4200/resetpassword/{token}'>Click me</a>\n";    
            string subject = "Email has sent for reset password";
            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("ritvikbridgelabz@gmail.com", "rgdircfcudbzxeil"),
                EnableSsl = true,
            };
            smtp.Send("ritvikbridgelabz@gmail.com", "ritvikbridgelabz@gmail.com", subject, body);
            fundoQueue.BeginReceive();
        }
    }
}       
