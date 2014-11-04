using System.Collections.Generic;
using System.Data.Linq.Mapping;

namespace DomainModel.Entities
{
    [Table(Name = "CountyCity")]
    public class CountyCity
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int CountyCityId { get; set; }
        [Column]
        public string County { get; set; }
        [Column]
        public string City { get; set; }
    }
}
