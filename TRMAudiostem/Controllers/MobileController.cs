using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TRMAudiostem.Controllers
{
    public class MobileController : Controller
    {
        private TRMWebService.TRMWCFWebServiceJson TRMWebService = new TRMWebService.TRMWCFWebServiceJson();
        //
        // GET: /Mobile/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Profile()
        {
            return View();
        }

        public ActionResult Venues()
        {
            var businessUsers = TRMWebService.GetAllBusinesses();            

            return View(businessUsers);
        }

        public ActionResult VenuePlaylist(int userId)
        {
            var businessUserActivePlaylist = TRMWebService.GetPlaylistsByUserId(userId).FirstOrDefault(x => x.Active == true);

            return View(businessUserActivePlaylist);
        }
    }
}
