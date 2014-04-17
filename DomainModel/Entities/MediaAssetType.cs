using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    [Table(Name="MediaAssetType")]
    public class MediaAssetType
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int MediaAssetTypeId { get; set; }
        [Column]
        public string MediaAssetTypeName { get; set; }
    }
}
