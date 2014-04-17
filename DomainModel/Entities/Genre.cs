using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    [Table(Name="Genre")]
    public class Genre
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int GenreId { get; set; }
        [Column]
        public string GenreName { get; set; }
        public bool Selected { get; set; }
    }
}
