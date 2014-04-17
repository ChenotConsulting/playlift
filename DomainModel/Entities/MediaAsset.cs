using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    [Table(Name="MediaAsset")]
    public class MediaAsset
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int MediaAssetId { get; set; }
        [Column]
        public string MediaAssetFileName { get; set; }
        [Column]
        public int MediaAssetFileSize { get; set; }
        [Column]
        public int MediaAssetDuration { get; set; }
        [Column]
        public int MediaAssetTypeId { get; set; }
        [Column]
        public int MediaAssetFormatId { get; set; }
        [Column]
        public DateTime CreatedDate { get; set; }

        public MediaAssetType MediaAssetType { get; set; }
        public MediaAssetFormat MediaAssetFormat { get; set; }
        public MediaAssetLocation MediaAssetLocation { get; set; }
    }
}
