using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PXLBusiness
{
    public class MailBusiness
    {
        public MailBusiness(MailData md)
        {
            email = md;
        }
        MailData email;
        public void SendEmail()
        {            
            MailMessage message = new MailMessage(email.MailFrom, email.MailTo);
            message.Subject = email.MailSubject;
            message.Body = email.MailBody;
            var client = new SmtpClient(email.MailServer, email.MailServerPort)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(email.MailFrom, email.MailPwd),
                EnableSsl = true
            };
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
    public class MailData
    {
        public MailData()
        {
            MailServer = "smtp.gmail.com";
            MailServerPort = 587;
            MailFrom = "Info.RoomyWebshop@gmail.com";// your team gmail account
            MailPwd = "R00m13123";//your team account pwd
        }
        public string MailServer { get; set; }
        public int MailServerPort { get; set; }
        public string MailTo { get; set; }
        public string MailFrom { get; set; }
        public string MailPwd { get; set; }
        public string MailBody { get; set; }
        public string MailSubject { get; set; }
    }
}
