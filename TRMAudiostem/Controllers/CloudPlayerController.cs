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
    [Authorize(Roles = "Admin, Business")]
    public class CloudPlayerController : Controller
    {
        private TRMWebService.TRMWCFWebServiceJson TRMWebService = new TRMWebService.TRMWCFWebServiceJson();

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

            var userPlaylistCollection = TRMWebService.GetPlaylistsByUserId(userIdToUse);

            return PartialView(userPlaylistCollection);
        }

        public JsonResult GetPlaylistSongs(int playlistId)
        {
            var songCollection = TRMWebService.GetAllPlaylistSongs(playlistId);
            var songModel = new List<ArtistSongModel>();

            foreach (var song in songCollection)
            {
                var album = TRMWebService.GetAlbumFromSongId(song.SongId);
                var artist = TRMWebService.GetArtistFromAlbumId(album.AlbumId);

                songModel.Add(new TRMAudiostem.Models.ArtistSongModel
                {
                    AlbumId = album.AlbumId,
                    AlbumCover = AudiostemBase.StreamingUrl + album.AlbumCover,
                    AlbumTitle = album.AlbumTitle,
                    ArtistName = artist.ArtistName,
                    MediaAssetPath = AudiostemBase.StreamingUrl + TRMWebService.GetSongMediaAssets(song.SongId).FirstOrDefault().MediaAssetLocation.Path,
                    SongComposer = song.SongComposer,
                    SongId = song.SongId,
                    SongReleaseDate = song.SongReleaseDate,
                    SongTitle = song.SongTitle
                });
            }

            if (TRMWebService.DeactivateUserPlaylists(WebSecurity.CurrentUserId) && TRMWebService.ActivateCurrentPlaylist(playlistId))
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
            var result = "The playlist could not be saved. Please try again. If the problem persists, please contact us at support@totalresolutionmusic.com";
            var formCollection = form.Split('&');

            if (!string.IsNullOrEmpty(AudiostemBase.ReturnFormItemValue(formCollection, "playlistName")) && !(AudiostemBase.ReturnFormItemValue(formCollection, "playlistName").Equals("New Playlist")))
            {
                // check if a playlist with this name already exists
                var playlist = TRMWebService.GetPlaylistByName(AudiostemBase.ReturnFormItemValue(formCollection, "playlistName"));

                var playlistId = 0;
                if (playlist.PlaylistId > 0)
                {
                    // then check if it belongs to this user
                    var userPlaylist = TRMWebService.GetUserPlaylistsByUserId(WebSecurity.CurrentUserId).Where(x => x.PlaylistId == playlist.PlaylistId).FirstOrDefault();

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

                if (playlistId == 0 && !string.IsNullOrEmpty(AudiostemBase.ReturnFormItemValue(formCollection, "playlistId")))
                {
                    if (int.TryParse(AudiostemBase.ReturnFormItemValue(formCollection, "playlistId"), out playlistId))
                    {
                        playlistId = Convert.ToInt32(AudiostemBase.ReturnFormItemValue(formCollection, "playlistId"));
                    }
                }

                var playlistSongCollection = PopulatePlaylistSongs(formCollection, playlistId);

                playlist = new Playlist()
                {
                    Active = true,
                    CreatedDate = DateTime.Now,
                    PlaylistId = playlistId,
                    PlaylistName = AudiostemBase.ReturnFormItemValue(formCollection, "playlistName"),
                    PlaylistSongCollection = playlistSongCollection
                };

                try
                {
                    playlistId = TRMWebService.SavePlaylist(WebSecurity.CurrentUserId, playlist);
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

            var playlistId = Convert.ToInt32(AudiostemBase.ReturnFormItemValue(formCollection, "playlistId"));

            if (playlistId > 0)
            {
                if (TRMWebService.RecordSongPlay(songId, playlistId))
                {
                    result = "success";
                }
            }

            return result;
        }

        [HttpPost]
        public string RemoveSongFromPlaylist(int songId, int playlistId, int position)
        {
            var result = "Warning! This song could not be removed. Please try again. If the problem persists, please contact us at support@totalresolutionmusic.com";

            try
            {
                TRMWebService.DeletePlaylistSong(songId, playlistId, position);
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
            var playlistSongCollection = TRMWebService.GetPlaylistById(playlistId).PlaylistSongCollection;

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
                    TRMWebService.DeletePlaylistSong(playlistSong.SongId, playlistId, playlistSong.Position);
                    playlistSongCollection.Remove(new PlaylistSong() { PlaylistSongId = playlistSong.PlaylistSongId });
                }
            }

            return playlistSongCollection;
        }
    }
}


