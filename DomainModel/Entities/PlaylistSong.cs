using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    [Table(Name="PlaylistSong")]
    public class PlaylistSong
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int PlaylistSongId { get; set; }
        [Column]
        public int PlaylistId { get; set; }
        [Column]
        public int SongId { get; set; }
        [Column]
        public DateTime DateAdded { get; set; }
        [Column]
        public int Position { get; set; }
    }
}
