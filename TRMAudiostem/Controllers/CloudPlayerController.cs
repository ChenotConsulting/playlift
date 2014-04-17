using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TRMAudiostem.Models;

namespace TRMAudiostem.Controllers
{
    [Authorize(Roles="Admin")]
    public class CloudPlayerController : Controller
    {
        private TRMWebService.TRMWCFWebServiceJson TRMWebService;

        //
        // GET: /CloudPlayer/

        public ActionResult Index()
        {
            ViewBag.CurrentImagePath = AudiostemBase.StreamingUrl + "";

            return View();
        }

        // Artists
        public PartialViewResult _Artists()
        {
            TRMWebService = new TRMWebService.TRMWCFWebServiceJson();
            var artists = TRMWebService.GetAllArtists();

            return PartialView(artists.Where(x => x.Active == true && x.SongCollection.Count > 0).ToList());
        }

        // Albums
        public PartialViewResult _Albums(List<Album> albums, Artist artist)
        {
            if (albums == null)
            {
                return PartialView(new List<Album>());
            }

            ViewBag.ArtistId = artist.ArtistId;
            ViewBag.ArtistName = artist.ArtistName;

            return PartialView(albums);
        }

        // Songs
        public PartialViewResult _AlbumSongs(Album album, string artistName)
        {
            TRMWebService = new TRMWebService.TRMWCFWebServiceJson();
            var songs = TRMWebService.GetAlbumSongs(album.AlbumId);
            var songModels = new List<ArtistSongModel>();

            ViewBag.AlbumCover = album.AlbumCover;
            ViewBag.AlbumId = album.AlbumId;
            ViewBag.AlbumTitle = album.AlbumTitle;
            ViewBag.ArtistName = artistName;

            foreach (var song in songs)
            {
                songModels.Add(new TRMAudiostem.Models.ArtistSongModel
                {
                    AlbumId = album.AlbumId,
                    MediaAssetPath = TRMWebService.GetSongMediaAssets(song.SongId).FirstOrDefault().MediaAssetLocation.Path,
                    SongComposer = song.SongComposer,
                    SongId = song.SongId,
                    SongReleaseDate = song.SongReleaseDate,
                    SongTitle = song.SongTitle
                });
            }

            return PartialView(songModels);
        }

        // Search
        public PartialViewResult _Search()
        {
            return PartialView();
        }

        [HttpPost]
        public PartialViewResult _Search(FormCollection form)
        {
            return PartialView();
        }
    }
}
