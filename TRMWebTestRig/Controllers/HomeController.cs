using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TRMWebTestRig.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Use this template to test AJAX calls to the WCF REST web service";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult CreatePlaylist(FormCollection formCollection)
        {
            return View("Index");
        }

        public FileResult DownloadFile(string url)
        {
            Response.ContentType = "application/octet-stream";
            return File(url, Response.ContentType);
        }
    }
}
