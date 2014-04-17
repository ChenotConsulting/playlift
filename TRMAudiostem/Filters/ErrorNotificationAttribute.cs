using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;

namespace TRMAudiostem.Filters
{
    public class ErrorNotificationAttribute : ActionFilterAttribute
    {
        public void OnException(ExceptionContext filterContext)
        {

            // Don't interfere if the exception is already handled
            if (filterContext.ExceptionHandled)
                return;

            //.. log exception and do appropriate notifications here
            MailMessage mailMsg = new MailMessage();

            mailMsg.To.Add(new MailAddress("jpchenot@gmail.com"));
            mailMsg.Body = filterContext.Exception.ToString();

            mailMsg.From = new MailAddress("info@totalresolutionmusic.com");

            mailMsg.Subject = "music.audiostem.co.uk - Exception";

            SmtpClient client = new SmtpClient("auth.smtp.1and1.co.uk");
            client.Credentials = new NetworkCredential("info@totalresolutionmusic.com", "trm_info");
            client.Send(mailMsg);
        }
    }
}
