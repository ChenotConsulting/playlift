using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    [Table(Name="ArtistGenre")]
    public class ArtistGenre
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int ArtistGenreId { get; set; }
        [Column]
        public int UserId { get; set; }
        [Column]
        public int GenreId { get; set; }
    }
}
