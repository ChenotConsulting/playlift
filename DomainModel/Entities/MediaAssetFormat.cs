using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    [Table(Name="MediaAssetFormat")]
    public class MediaAssetFormat
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int MediaAssetFormatId { get; set; }
        [Column]
        public string MediaAssetFormatName { get; set; }
    }
}
