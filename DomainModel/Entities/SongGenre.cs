using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    [Table(Name="SongGenre")]
    public class SongGenre
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int SongGenreId { get; set; }
        [Column]
        public int SongId { get; set; }
        [Column]
        public int GenreId { get; set; }
    }
}
