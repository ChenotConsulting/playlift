using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    [Table(Name="BusinessUser")]
    public class BusinessUser
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int BusinessUserId { get; set; }
        [Column]
        public int UserId { get; set; }
        [Column]
        public DateTime CreatedDate { get; set; }
        [Column]
        public int BusinessTypeId { get; set; }
        [Column]
        public string Address1 { get; set; }
        [Column]
        public string Address2 { get; set; }
        [Column]
        public string City { get; set; }
        [Column]
        public string Country { get; set; }
        [Column]
        public string PostCode { get; set; }
        [Column]
        public string Logo { get; set; }

        public BusinessType BusinessType { get; set; }
        public List<Playlist> PlaylistCollection { get; set; }
    }
}
