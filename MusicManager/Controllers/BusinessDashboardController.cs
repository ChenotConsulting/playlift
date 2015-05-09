using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicManager.Models;
using WebService;
using WebMatrix.WebData;

namespace MusicManager.Controllers
{
    public class BusinessDashboardController : Controller
    {
        private WCFWebServiceJson trmservice = new WCFWebServiceJson();

        //
        // GET: /BusinessDashboard/

        public ActionResult Index()
        {
            return PartialView();
        }

        public PartialViewResult _Artists()
        {
            var artistsModel = new List<DashboardArtistModel>();
            var artistCollection = trmservice.GetArtistsPlayedByVenue(WebSecurity.CurrentUserId);

            foreach (var artist in artistCollection)
            {
                var playCount = GetArtistTotalSongsPlay(artist.UserId);
                if (playCount > 0)
                {
                    artistsModel.Add(new DashboardArtistModel()
                    {
                        ArtistId = artist.ArtistId,
                        ArtistName = artist.ArtistName,
                        ProfileImage = artist.ProfileImage,
                        TimesPurchased = playCount,
                        UserId = artist.UserId
                    });
                }
            }

            return PartialView(artistsModel);
        }

        public PartialViewResult _Activity(int userId, string artistName)
        {
            ViewBag.ArtistName = artistName;

            List<Song> songCountByArtist = trmservice.GetSongCountByArtistAndVenue(userId, WebSecurity.CurrentUserId);
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

        private int GetArtistTotalSongsPlay(int artistUserId)
        {
            List<Song> songCountByArtist = trmservice.GetSongCountByArtistAndVenue(artistUserId, WebSecurity.CurrentUserId);
            IEnumerable<Song> songs = songCountByArtist.Distinct<Song>();
            var timesPurchased = 0;
            foreach (var song in songs)
            {
                timesPurchased += (
                    from x in songCountByArtist
                    where x.SongId == song.SongId
                    select x).Count<Song>();
            }

            return timesPurchased;
        }
    }
}
