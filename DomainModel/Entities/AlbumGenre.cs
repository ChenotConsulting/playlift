using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    [Table(Name="AlbumGenre")]
    public class AlbumGenre
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int AlbumGenreId { get; set; }
        [Column]
        public int AlbumId { get; set; }
        [Column]
        public int GenreId { get; set; }
    }
}
