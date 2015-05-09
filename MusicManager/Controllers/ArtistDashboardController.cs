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
    public class ArtistDashboardController : Controller
    {
        private WCFWebServiceJson trmservice = new WCFWebServiceJson();

        //
        // GET: /ArtistDashboard/

        public ActionResult Index()
        {
            return PartialView();
        }

        public PartialViewResult _SongsPlayed()
        {
            List<Song> songCountByArtist = trmservice.GetSongCountByArtist(WebSecurity.CurrentUserId);
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

        public PartialViewResult _Venues()
        {
            var dashboardVenueModelCollection = new List<DashboardVenueModel>();

            var venueCollection = trmservice.GetAllBusinesses();

            foreach (var venue in venueCollection)
            {
                var songCountByVenue = this.trmservice.GetSongCountByVenue(venue.UserId, WebSecurity.CurrentUserId);
                IEnumerable<Song> songs = songCountByVenue.Distinct<Song>();

                foreach (var song in songs)
                {
                    var dashboardSongModel = new DashboardVenueModel()
                    {
                        Venue = venue,
                        CreatedDate = song.CreatedDate,
                        PRS = song.PRS,
                        SongComposer = song.SongComposer,
                        SongId = song.SongId,
                        SongReleaseDate = song.SongReleaseDate,
                        SongTitle = song.SongTitle,
                        TimesPurchased = (
                            from x in songCountByVenue
                            where x.SongId == song.SongId
                            select x).Count<Song>(),
                        AlbumCollection = song.AlbumCollection
                    };
                    dashboardVenueModelCollection.Add(dashboardSongModel);
                }
            }

            return PartialView(dashboardVenueModelCollection);
        }

        public PartialViewResult _VenuesMap()
        {
            var venueCollection = trmservice.GetAllBusinesses();

            return PartialView(venueCollection);
        }

        public PartialViewResult _VenueMap(int userId)
        {
            var venue = trmservice.GetBusiness(userId);

            return PartialView(venue);
        }

        public PartialViewResult _SongsTime(int songId)
        {
            ViewBag.SongTitle = trmservice.GetAllSongs().Where(x => x.SongId == songId).Select(x => x.SongTitle).FirstOrDefault();
            ViewBag.SongId = songId;
            var purchasedSongCollection = trmservice.GetPurchasedSongs(songId);

            return PartialView(purchasedSongCollection);
        }

        public PartialViewResult _SongsHours(int songId)
        {
            ViewBag.SongTitle = trmservice.GetAllSongs().Where(x => x.SongId == songId).Select(x => x.SongTitle).FirstOrDefault();
            ViewBag.SongId = songId;
            var purchasedSongCollection = trmservice.GetPurchasedSongs(songId);
            var purchasedSongCountCollection = purchasedSongCollection.Select(x => x.DatePurchased.ToString("HH"));
            var purchasedSongCountModelCollection = new List<PurchasedSongCountModel>();
            IEnumerable<string> purchasedSongs = purchasedSongCountCollection.Distinct<string>();

            foreach (var purchasedSongCount in purchasedSongs)
            {
                purchasedSongCountModelCollection.Add(new PurchasedSongCountModel()
                {
                    HoursPurchased = int.Parse(purchasedSongCount),
                    CountPerHour = (from x in purchasedSongCountCollection
                            where x == purchasedSongCount
                            select x).Count<string>()
                });
            }

            return PartialView(purchasedSongCountModelCollection.ToList());
        }
    }
}
