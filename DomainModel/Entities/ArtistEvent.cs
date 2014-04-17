using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    [Table(Name="ArtistEvent")]
    public class ArtistEvent
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int ArtistEventId { get; set; }
        [Column]
        public int ArtistId { get; set; }
        [Column]
        public int EventId { get; set; }
    }
}
