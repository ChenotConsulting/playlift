using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TRMInfrastructure.Utilities
{
    public class Utilities
    {
        public string RemoveSpaces(string input)
        {
            input = input.Replace(" ", "_").Replace("+", "_");

            return input.Trim();
        }

        public void ErrorNotification(Exception ex)
        {
            try
            {
                LogError(ex);

                MailMessage mailMsg = new MailMessage();

                mailMsg.To.Add(new MailAddress("jpchenot@gmail.com"));
                mailMsg.Body = ex.ToString();

                mailMsg.From = new MailAddress("info@totalresolutionmusic.com");

                mailMsg.Subject = HttpContext.Current.Request.Url.ToString() + " - Exception";

                SmtpClient client = new SmtpClient("auth.smtp.1and1.co.uk");
                client.Credentials = new NetworkCredential("info@totalresolutionmusic.com", "trm_info");
                client.Send(mailMsg);
            }
            catch
            {
                throw;
            }
        }

        private void LogError(Exception ex)
        {
            string filePath = @"E:\temp\error_log_" + DateTime.Now.ToString("ddMMyyyy") + "_at_" + DateTime.Now.ToString("hhmmss") + ".txt";

            File.WriteAllText(filePath, ex.ToString());
        }

        public void LogMessage(string type, string message)
        {
            string filePath = @"E:\temp\app_" + type + "_log_" + DateTime.Now.ToString("ddMMyyyy") + "_at_" + DateTime.Now.ToString("hhmmss") + ".txt";

            File.WriteAllText(filePath, message);
        }
    }
}
