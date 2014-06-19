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

        [Display(Name = "Release Date")]
        public DateTime SongReleaseDate { get; set; }

        [Display(Name = "Title")]
        public string SongTitle { get; set; }

        [Display(Name = "Composer")]
        public string SongComposer { get; set; }

        [Display(Name = "Registered with PRS?")]
        public bool PRS { get; set; }

        [Display(Name = "Media File")]
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
}