using System.Collections.Generic;
using System.Data.Linq.Mapping;

namespace DomainModel.Entities
{
    [Table(Name = "Artist")]
    public class Artist : User
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int ArtistId { get; set; }
        [Column]
        public string ArtistName { get; set; }
        [Column]
        public int UserId { get; set; }
        [Column]
        public string Email { get; set; }
        [Column]
        public string Website { get; set; }
        [Column]
        public string SoundCloud { get; set; }
        [Column]
        public string MySpace { get; set; }
        [Column]
        public string Facebook { get; set; }
        [Column]
        public string Twitter { get; set; }
        [Column]
        public bool TermsAndConditionsAccepted { get; set; }
        [Column]
        public string ProfileImage { get; set; }
        [Column]
        public bool PRS { get; set; }
        [Column]
        public bool CreativeCommonsLicence { get; set; }
        [Column]
        public bool Active { get; set; }
        [Column]
        public string Bio { get; set; }

        public List<Genre> GenreCollection { get; set; }
        public List<Song> SongCollection { get; set; }
        public List<Album> AlbumCollection { get; set; }
        public List<Event> EventCollection { get; set; }
    }
}
