using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TRMAudiostem.Models;
using TRMWebService;
using WebMatrix.WebData;

namespace TRMAudiostem.Controllers
{
    public class ArtistDashboardController : Controller
    {
        private TRMWCFWebServiceJson trmservice = new TRMWCFWebServiceJson();

        //
        // GET: /ArtistDashboard/

        public ActionResult Index()
        {
            List<Song> songCountByArtist = this.trmservice.GetSongCountByArtist(WebSecurity.CurrentUserId);
            IEnumerable<Song> songs = songCountByArtist.Distinct<Song>();
            List<DashboardSongModel> dashboardSongModelCollection = new List<DashboardSongModel>();
            foreach (var song in songs)
            {
                var dashboardSongModel = new DashboardSongModel()
                {
                    CreatedDate = song.CreatedDate,
                    PRS = song.PRS,
                    SongComposer = song.SongComposer,
                    SongId = song.SongId,
                    SongReleaseDate = song.SongReleaseDate,
                    SongTitle = song.SongTitle,
                    TimesPurchased = (
                        from x in songCountByArtist
                        where x.SongId == song.SongId
                        select x).Count<Song>(),
                    AlbumCollection = song.AlbumCollection
                };
                dashboardSongModelCollection.Add(dashboardSongModel);
            }
            return PartialView(dashboardSongModelCollection);
        }

    }
}
