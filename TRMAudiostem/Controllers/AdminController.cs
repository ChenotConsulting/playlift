using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TRMAudiostem.Models;
using WebMatrix.WebData;

namespace TRMAudiostem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            var trmwebservice = new TRMWebService.TRMWCFWebServiceJson();

            return View(trmwebservice.GetAllArtists());
        }

        [HttpPost]
        public bool ActivateArtist(int userId)
        {
            var trmwebservice = new TRMWebService.TRMWCFWebServiceJson();
            var artist = trmwebservice.GetArtist(userId);
            artist.Active = true;
            return trmwebservice.UpdateArtist(artist, artist.GenreCollection, null);
        }

        [HttpPost]
        public bool DeactivateArtist(int userId)
        {
            var trmwebservice = new TRMWebService.TRMWCFWebServiceJson();
            var artist = trmwebservice.GetArtist(userId);
            artist.Active = false;
            return trmwebservice.UpdateArtist(artist, artist.GenreCollection, null);
        }

        //
        // GET: Artist details

        public ActionResult ArtistProfile(int userId)
        {
            var trmwebservice = new TRMWebService.TRMWCFWebServiceJson();
            var artist = trmwebservice.GetArtist(userId);

            ViewBag.ImagePath = AudiostemBase.StreamingUrl + artist.ProfileImage;

            return View(artist);
        }

        public ActionResult ArtistAlbums(int userId)
        {
            var trmservice = new TRMWebService.TRMWCFWebServiceJson();
            var artist = trmservice.GetArtist(userId);
            ViewBag.ArtistAlbums = trmservice.GetArtistAlbums(artist);
            ViewBag.UserId = userId;

            return View();
        }

        public ActionResult ViewAlbum(Album album)
        {
            ViewBag.AlbumCoverPath = AudiostemBase.StreamingUrl + album.AlbumCover;
            return View(album);
        }

        public ActionResult ArtistSongs(int userId)
        {
            var trmservice = new TRMWebService.TRMWCFWebServiceJson();
            var artist = trmservice.GetArtist(userId);
            ViewBag.ArtistAlbums = trmservice.GetArtistAlbums(artist);
            ViewBag.UserId = userId;

            return View();
        }

        public ActionResult ArtistDetails(int userId)
        {
            var TrmWcfWebServiceJson = new TRMWebService.TRMWCFWebServiceJson();
            var artist = TrmWcfWebServiceJson.GetArtist(userId);

            return View(artist);
        }
    }
}
