using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    [Table(Name="Playlist")]
    public class Playlist
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int PlaylistId { get; set; }
        [Column]
        public string PlaylistName { get; set; }
        [Column]
        public DateTime CreatedDate { get; set; }
        [Column]
        public bool Active { get; set; }

        public List<PlaylistSong> PlaylistSongCollection { get; set; }
    }
}
