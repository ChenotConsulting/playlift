
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    [Table(Name="Album")]
    public class Album
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int AlbumId { get; set; }
        [Column]
        public DateTime AlbumReleaseDate { get; set; }
        [Column]
        public string AlbumTitle { get; set; }
        [Column]
        public string AlbumProducer { get; set; }
        [Column]
        public string AlbumLabel { get; set; }
        [Column]
        public string AlbumCover { get; set; }
        [Column]
        public DateTime CreatedDate { get; set; }

        public List<Genre> GenreCollection { get; set; }
    }
}
