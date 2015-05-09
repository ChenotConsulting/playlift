using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using MusicManager.Models;
using System.Configuration;
using System.Web;
using System.Web.Routing;

namespace MusicManager
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");            

            OAuthWebSecurity.RegisterFacebookClient(
            appId: ConfigurationManager.AppSettings["FacebookAppID"],
            appSecret: ConfigurationManager.AppSettings["FacebookAppSecret"]);

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "ur92nmYMvp1LTqHyX3yXA",
            //    consumerSecret: "7HqwUoB2bcanfl7K7uW4M3mVu3vwo3YwkEeaDX47s");

            OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
