using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel.Abstract;
using DomainModel.Entities;

namespace DomainModel.Concrete
{
    public class SqlMediaAssetFormatRepository : IMediaAssetFormatRepository
    {
        public Table<MediaAssetFormat> MediaAssetFormatTable;
        public SqlMediaAssetFormatRepository(string connString)
        {
            MediaAssetFormatTable = (new DataContext(connString)).GetTable<MediaAssetFormat>();
        }

        public IQueryable<MediaAssetFormat> MediaAssetFormat { get { return MediaAssetFormatTable; } }
        public MediaAssetFormat GetMediaAssetFormatById(int mediaAssetFormatId)
        {
            return MediaAssetFormatTable.FirstOrDefault(x => x.MediaAssetFormatId == mediaAssetFormatId);
        }
    }
}
