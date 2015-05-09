using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicManager.Models;
using WebMatrix.WebData;

namespace MusicManager.Controllers
{
    [Authorize(Roles = "Admin, Business")]
    public class CloudPlayerController : Controller
    {
        private WebService.WCFWebServiceJson WebService = new WebService.WCFWebServiceJson();

        //
        // GET: /CloudPlayer/

        public ActionResult Index()
        {
            ViewBag.CurrentImagePath = MusicManagerBase.StreamingUrl + "";

            return View();
        }

        // Artists
        public PartialViewResult _Artists()
        {
            var artists = WebService.GetAllArtists();

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
            var songs = WebService.GetAlbumSongs(album.AlbumId);
            var songModels = new List<ArtistSongModel>();

            ViewBag.AlbumCover = album.AlbumCover;
            ViewBag.AlbumId = album.AlbumId;
            ViewBag.AlbumTitle = album.AlbumTitle;
            ViewBag.ArtistName = artistName;

            foreach (var song in songs)
            {
                songModels.Add(new MusicManager.Models.ArtistSongModel
                {
                    AlbumId = album.AlbumId,
                    MediaAssetPath = WebService.GetSongMediaAssets(song.SongId).FirstOrDefault().MediaAssetLocation.Path,
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

        // Playlist
        public PartialViewResult _UserPlaylists(int? userId)
        {
            var userIdToUse = 0;
            if (userId == null || userId == 0)
            {
                userIdToUse = WebSecurity.CurrentUserId;
            }
            else
            {
                userIdToUse = (int)userId;
            }

            var userPlaylistCollection = WebService.GetPlaylistsByUserId(userIdToUse);

            return PartialView(userPlaylistCollection);
        }

        public JsonResult GetPlaylistSongs(int playlistId)
        {
            var songCollection = WebService.GetAllPlaylistSongs(playlistId);
            var songModel = new List<ArtistSongModel>();

            foreach (var song in songCollection)
            {
                var album = WebService.GetAlbumFromSongId(song.SongId);
                var artist = WebService.GetArtistFromAlbumId(album.AlbumId);

                songModel.Add(new MusicManager.Models.ArtistSongModel
                {
                    AlbumId = album.AlbumId,
                    AlbumCover = MusicManagerBase.StreamingUrl + album.AlbumCover,
                    AlbumTitle = album.AlbumTitle,
                    ArtistName = artist.ArtistName,
                    MediaAssetPath = MusicManagerBase.StreamingUrl + WebService.GetSongMediaAssets(song.SongId).FirstOrDefault().MediaAssetLocation.Path,
                    SongComposer = song.SongComposer,
                    SongId = song.SongId,
                    SongReleaseDate = song.SongReleaseDate,
                    SongTitle = song.SongTitle
                });
            }

            if (WebService.DeactivateUserPlaylists(WebSecurity.CurrentUserId) && WebService.ActivateCurrentPlaylist(playlistId))
            {
                return Json(songModel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public string SavePlaylist(string form, bool newPlaylist)
        {
            var result = "The playlist could not be saved. Please try again. If the problem persists, please contact us at support@playlift.co.uk";
            var formCollection = form.Split('&');

            if (!string.IsNullOrEmpty(MusicManagerBase.ReturnFormItemValue(formCollection, "playlistName")) && !(MusicManagerBase.ReturnFormItemValue(formCollection, "playlistName").Equals("New Playlist")))
            {
                // check if a playlist with this name already exists
                var playlist = WebService.GetPlaylistByName(MusicManagerBase.ReturnFormItemValue(formCollection, "playlistName"));

                var playlistId = 0;
                if (playlist.PlaylistId > 0)
                {
                    // then check if it belongs to this user
                    var userPlaylist = WebService.GetUserPlaylistsByUserId(WebSecurity.CurrentUserId).Where(x => x.PlaylistId == playlist.PlaylistId).FirstOrDefault();

                    if (userPlaylist != null)
                    {
                        playlistId = playlist.PlaylistId;
                    }
                }

                if (playlistId > 0 && newPlaylist)
                {
                    result = "Warning! This name is already in use. Please select a new name or load the playlist with this name and add this song to it.";
                    return result;
                }

                if (playlistId == 0 && !string.IsNullOrEmpty(MusicManagerBase.ReturnFormItemValue(formCollection, "playlistId")))
                {
                    if (int.TryParse(MusicManagerBase.ReturnFormItemValue(formCollection, "playlistId"), out playlistId))
                    {
                        playlistId = Convert.ToInt32(MusicManagerBase.ReturnFormItemValue(formCollection, "playlistId"));
                    }
                }

                var playlistSongCollection = PopulatePlaylistSongs(formCollection, playlistId);

                playlist = new Playlist()
                {
                    Active = true,
                    CreatedDate = DateTime.Now,
                    PlaylistId = playlistId,
                    PlaylistName = MusicManagerBase.ReturnFormItemValue(formCollection, "playlistName"),
                    PlaylistSongCollection = playlistSongCollection
                };

                try
                {
                    playlistId = WebService.SavePlaylist(WebSecurity.CurrentUserId, playlist);
                    if (playlistId > 0)
                    {
                        result = "The playlist has been saved successfully!&" + playlistId;
                    }
                }
                catch (Exception ex)
                {
                    return result;
                }
            }
            else
            {
                result = "Please enter a valid name for your playlist!";
            }

            return result;
        }

        [HttpPost]
        public string SaveSongPlayCount(string form, int songId)
        {
            var result = "failed";
            var formCollection = form.Split('&');

            var playlistId = Convert.ToInt32(MusicManagerBase.ReturnFormItemValue(formCollection, "playlistId"));

            if (playlistId > 0)
            {
                if (WebService.RecordSongPlay(songId, playlistId))
                {
                    result = "success";
                }
            }

            return result;
        }

        [HttpPost]
        public string RemoveSongFromPlaylist(int songId, int playlistId, int position)
        {
            var result = "Warning! This song could not be removed. Please try again. If the problem persists, please contact us at support@playlift.co.uk";

            try
            {
                WebService.DeletePlaylistSong(songId, playlistId, position);
                result = "The song has been successfully removed from the playlist.";
            }
            catch (Exception ex)
            {
                return result;
            }

            return result;
        }

        private List<PlaylistSong> PopulatePlaylistSongs(string[] formCollection, int playlistId)
        {
            var playlistSongCollection = DeleteRemovedPlaylistSongs(formCollection, playlistId);

            for (int i = 0; i < formCollection.Length; i++)
            {
                if (formCollection[i].StartsWith("song"))
                {
                    // get the position of the song
                    var positionIndex = formCollection[i].IndexOf('_');
                    var position = Convert.ToInt32(formCollection[i].Substring(positionIndex + 1));
                    var songIdSubstring = formCollection[i].Substring(0, positionIndex);

                    // now get the song Id
                    var songIdIndex = songIdSubstring.IndexOf('=');
                    var songId = Convert.ToInt32(songIdSubstring.Substring(songIdIndex + 1));

                    if (!playlistSongCollection.Any(p => p.SongId == songId))
                    {
                        playlistSongCollection.Add(new PlaylistSong()
                        {
                            DateAdded = DateTime.Now,
                            PlaylistId = playlistId,
                            Position = position,
                            SongId = songId
                        });
                    }
                }
            }

            return playlistSongCollection;
        }

        private List<PlaylistSong> DeleteRemovedPlaylistSongs(string[] formCollection, int playlistId)
        {
            var playlistSongCollection = WebService.GetPlaylistById(playlistId).PlaylistSongCollection;

            foreach (var playlistSong in playlistSongCollection)
            {
                var songExist = false;
                for (int i = 0; i < formCollection.Length; i++)
                {
                    if (formCollection[i].StartsWith("song"))
                    {
                        // remove the position from the string
                        var positionIndex = formCollection[i].IndexOf('_');
                        var songIdSubstring = formCollection[i].Substring(0, positionIndex);

                        // get the song Id from the remaining string
                        var index = songIdSubstring.IndexOf('=');
                        var songId = Convert.ToInt32(songIdSubstring.Substring(index + 1));

                        if (playlistSong.SongId == songId)
                        {
                            songExist = true;
                        }
                    }
                }

                if (!songExist)
                {
                    WebService.DeletePlaylistSong(playlistSong.SongId, playlistId, playlistSong.Position);
                    playlistSongCollection.Remove(new PlaylistSong() { PlaylistSongId = playlistSong.PlaylistSongId });
                }
            }

            return playlistSongCollection;
        }
    }
}


