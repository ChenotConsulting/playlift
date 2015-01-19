using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMWebService;

namespace DataGenerator
{
    class Program
    {
        private TRMWebService.TRMWCFWebServiceJson TRMWebService = new TRMWebService.TRMWCFWebServiceJson();

        static void Main(string[] args)
        {
            var trmservice = new TRMWCFWebServiceJson();
            var random = new Random();

            // get all songs in playlists
            var songCollection = trmservice.GetAllPlaylistSongs();

            var songCollectionCount = songCollection.Count();            
            var randomPlaylistSongIndex = random.Next(0, songCollectionCount - 1);

            var playlistSong = songCollection.ToArray()[randomPlaylistSongIndex];

            // get the user Id from the playlist
            var userPlaylist = trmservice.GetUserPlaylistByPlaylistId(playlistSong.PlaylistId);

            // get all business users and choose one at random
            //var businessUsers = trmservice.GetAllBusinesses();

            //var businessUsersCount = businessUsers.Count();
            //var randomBusinessUserIndex = random.Next(0, businessUsersCount - 1);

            //var businessUser = businessUsers.ToArray()[randomBusinessUserIndex];

            trmservice.RecordSongPlayByUser(playlistSong.SongId, playlistSong.PlaylistId, userPlaylist.UserId);
        }
    }
}
