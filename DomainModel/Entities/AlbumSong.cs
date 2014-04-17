using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    [Table(Name="AlbumSong")]
    public class AlbumSong
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int AlbumSongId { get; set; }
        [Column]
        public int AlbumId { get; set; }
        [Column]
        public int SongId { get; set; }
    }
}
