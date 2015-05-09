using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicManager.Controllers
{
    public class MobileController : Controller
    {
        private WebService.WCFWebServiceJson WebService = new WebService.WCFWebServiceJson();
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
            var businessUsers = WebService.GetAllBusinesses();            

            return View(businessUsers);
        }

        public ActionResult VenuePlaylist(int userId)
        {
            var businessUserActivePlaylist = WebService.GetPlaylistsByUserId(userId).FirstOrDefault(x => x.Active == true);

            return View(businessUserActivePlaylist);
        }
    }
}
