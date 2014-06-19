using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    [Table(Name="Song")]
    public class Song
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int SongId { get; set; }
        [Column]
        public string SongTitle { get; set; }
        [Column]
        public DateTime SongReleaseDate { get; set; }
        [Column]
        public string SongComposer { get; set; }
        [Column]
        public DateTime CreatedDate { get; set; }
        [Column]
        public bool PRS { get; set; }

        public List<MediaAsset> songMediaAsset { get; set; }
        public List<Genre> GenreCollection { get; set; }
        public List<Album> AlbumCollection { get; set; }
    }
}
