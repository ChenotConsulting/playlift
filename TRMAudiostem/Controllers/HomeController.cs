using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace TRMAudiostem.Controllers
{
    public class HomeController : Controller
    {
        private TRMWebService.TRMWCFWebServiceJson TRMWebService = new TRMWebService.TRMWCFWebServiceJson();

        public ActionResult Index()
        {
            ViewBag.Register = "Register as an artist or band";
            ViewBag.RegisterAction = "RegisterArtist";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Register = "Register as an artist or band";
            ViewBag.RegisterAction = "RegisterArtist";
            ViewBag.Message = "Audiostem.";

            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        public PartialViewResult TermsAndConditions()
        {
            return PartialView();
        }

        public PartialViewResult PrivacyPolicy()
        {
            return PartialView();
        }

        public PartialViewResult AcceptableUsePolicy()
        {
            return PartialView();
        }

        public ActionResult Artist()
        {
            ViewBag.Register = "Register as an artist or band";
            ViewBag.RegisterAction = "RegisterArtist";
            ViewBag.Message = "Artist.";

            return View();
        }

        public ActionResult Business()
        {
            ViewBag.Register = "Register as a business";
            ViewBag.RegisterAction = "RegisterBusiness";
            ViewBag.Message = "Business.";

            return View();
        }

        public ActionResult User()
        {
            ViewBag.Register = "Register as a personal user";
            ViewBag.RegisterAction = "Register";
            ViewBag.Message = "Personal user.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Register = "Register as an artist or band";
            ViewBag.RegisterAction = "RegisterArtist";
            ViewBag.Message = "Artist.";

            return View();
        }

        public ActionResult Faq()
        {
            ViewBag.Register = "Register as an artist or band";
            ViewBag.RegisterAction = "RegisterArtist";

            return View();
        }

        // Cloud player section

        [Authorize(Roles="Admin")]
        public ActionResult CloudPlayer()
        {
            var trmservice = new TRMWebService.TRMWCFWebServiceJson();
            var artist = new Artist(); //trmservice.GetArtist(WebSecurity.CurrentUserId);

            ViewBag.ImagePath = AudiostemBase.StreamingUrl + artist.ProfileImage;
            ViewBag.Genres = TRMWebService.GetAllGenres();

            return View();
        }

        public PartialViewResult ArtistList()
        {
            return PartialView();
        }

        public PartialViewResult ArtistAlbums()
        {
            return PartialView();
        }

        public PartialViewResult ArtistSongs()
        {
            return PartialView();
        }
    }
}
