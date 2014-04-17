using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    [Table(Name="MediaAssetLocation")]
    public class MediaAssetLocation
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int MediaAssetLocationId { get; set; }
        [Column]
        public int ProtocolId { get; set; }
        [Column]
        public string Path { get; set; }
        [Column]
        public int MediaAssetId { get; set; }

        public Protocol Protocol { get; set; }
    }
}
