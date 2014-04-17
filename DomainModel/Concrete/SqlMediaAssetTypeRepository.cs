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
    public class SqlMediaAssetTypeRepository : IMediaAssetTypeRepository
    {
        public Table<MediaAssetType> MediaAssetTypeTable;
        public SqlMediaAssetTypeRepository(string connString)
        {
            MediaAssetTypeTable = (new DataContext(connString)).GetTable<MediaAssetType>();
        }

        public IQueryable<MediaAssetType> MediaAssetType { get { return MediaAssetTypeTable; } }
        public MediaAssetType GetMediaAssetType(int mediaAssetTypeId)
        {
            return MediaAssetTypeTable.FirstOrDefault(x => x.MediaAssetTypeId == mediaAssetTypeId);
        }
    }
}
