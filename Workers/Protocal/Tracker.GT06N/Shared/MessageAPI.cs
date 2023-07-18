using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tracker.GT06N.Shared
{
    /// <summary>
    /// Message API for alerts in case of Violation of policy(s).
    /// </summary>
    public class MessageAPI
    {
        private readonly IConfiguration _configuration;
        public MessageAPI(IConfiguration configuration)
        {
            _configuration = configuration;
            //string filePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            //string exeDir = Path.GetDirectoryName(filePath);
            //log4net.ThreadContext.Properties["Directory"] = exeDir; //log file path
            //log4net.ThreadContext.Properties["vid"] = "Common"; //log file path
            //log4net.Config.XmlConfigurator.Configure();
            //logger = Logger<MessageAPI>().GetLogger(typeof(MessageAPI));
        }

        public struct NameEmailPair
        {
            public string Name;
            public string EmailID;
        }

        #region Email methods

        public string SendEmail(string subject, string message, string toAddress)
        {
            string returnString = string.Empty;
            var fromAddress = new MailAddress(_configuration.GetSection("Appsettings")["fromAddress"]);
            SmtpClient smtp = new SmtpClient
            {
                Host = _configuration.GetSection("Appsettings")["smtpServer"].ToString(),
                Port = int.Parse(_configuration.GetSection("Appsettings")["smtpPort"]),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_configuration.GetSection("Appsettings")["username"], _configuration.GetSection("Appsettings")["password"])
            };
            if (!string.IsNullOrEmpty(toAddress))
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = fromAddress;
                    mailMessage.To.Add(toAddress);
                    mailMessage.Subject = subject;
                    mailMessage.Body = message;
                    //mailMessage.IsBodyHtml = true;
                    try
                    {
                        //logger.Debug("Starting Email Sending");
                        smtp.Send(mailMessage);
                        //logger.Debug("Email Sending complete.");
                        returnString = "Email sent successfully.";
                    }
                    catch (Exception ex)
                    {
                        //logger.Error("Error is Email Sending", ex);
                        returnString = "Email sending failed.";
                    }
                }

            return returnString;
        }

        #endregion
    }
}
