using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Net;
using Infrastructure.Utilities;
using WebMatrix.WebData;

namespace MusicManager
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var util = new Utilities();
            util.ErrorNotification(Server.GetLastError());

            MailMessage mailMsg = new MailMessage();

            mailMsg.To.Add(new MailAddress("jpchenot@gmail.com"));
            mailMsg.Body = Server.GetLastError().ToString();

            mailMsg.From = new MailAddress("info@playlift.co.uk");

            mailMsg.Subject = HttpContext.Current.Request.Url.ToString() + " - Exception";

            SmtpClient client = new SmtpClient("auth.smtp.1and1.co.uk");
            client.Credentials = new NetworkCredential("info@playlift.co.uk", "trm_info");
            client.Send(mailMsg);
        }
    }
}