using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TRMWebService;

namespace TRMAudiostem.Models
{
    public class ArtistAlbumModel
    {
        public int AlbumId { get; set; }

        [Display(Name = "Release Date")]
        public DateTime AlbumReleaseDate { get; set; }

        [Display(Name = "Title")]
        public string AlbumTitle { get; set; }

        [Display(Name = "Producer")]
        public string AlbumProducer { get; set; }

        [Display(Name = "Label")]
        public string AlbumLabel { get; set; }

        [Display(Name = "Album Cover")]
        public HttpPostedFileBase AlbumCover { get; set; }

        [Display(Name = "Album Cover Image")]
        public string AlbumCoverPath { get; set; }

        [Display(Name = "Genres")]
        public IEnumerable<Genre> GenreCollection { get; set; }

        public IEnumerable<Genre> GenreList
        {
            get
            {
                var TRMWCFWebServiceJson = new TRMWebService.TRMWCFWebServiceJson();
                return TRMWCFWebServiceJson.GetAllGenres();
            }
        }
    }

    public class ArtistSongModel
    {
        public int SongId { get; set; }
        public int AlbumId { get; set; }
        public string AlbumCover { get; set; }
        public string AlbumTitle { get; set; }
        public string ArtistName { get; set; }

        [Display(Name = "Release Date")]
        public DateTime SongReleaseDate { get; set; }

        [Display(Name = "Title")]
        public string SongTitle { get; set; }

        [Display(Name = "Composer")]
        public string SongComposer { get; set; }

        [Display(Name = "Registered with PRS?")]
        public bool PRS { get; set; }

        [Display(Name = "Audio File")]
        public HttpPostedFileBase MediaAsset { get; set; }
        public string MediaAssetPath { get; set; }

        [Display(Name = "Genres")]
        public IEnumerable<Genre> GenreCollection { get; set; }

        public IEnumerable<Genre> GenreList
        {
            get
            {
                var TRMWCFWebServiceJson = new TRMWebService.TRMWCFWebServiceJson();
                return TRMWCFWebServiceJson.GetAllGenres();
            }
        }
    }

    public class PlaylistModel
    {
        public int PlaylistId { get; set; }

        [Display(Name = "Playlist Name")]
        public string PlaylistName { get; set; }

        [Display(Name = "Is Playlist Active?")]
        public bool Active { get; set; }
    }

    public class PlaylistSongModel
    {
        public int PlaylistSongId { get; set; }
        public int PlaylistId { get; set; }
        public int SongId { get; set; }
        public int Position { get; set; }
    }

    public class UserPlaylistModel
    {
        public int UserPlaylistId { get; set; }
        public int PlaylistId { get; set; }
        public int UserId { get; set; }
    }
}