using System;
using System.Data.Linq;
using System.Linq;
using DomainModel.Abstract;
using DomainModel.Entities;

namespace DomainModel.Concrete
{
    public class SqlMediaAssetRepository : IMediaAssetRepository
    {
        public Table<MediaAsset> MediaAssetTable;
        public SqlMediaAssetRepository(string connString)
        {
            MediaAssetTable = (new DataContext(connString)).GetTable<MediaAsset>();
        }

        public IQueryable<MediaAsset> MediaAsset { get { return MediaAssetTable; } }
        public int SaveMediaAsset(MediaAsset mediaAsset)
        {
            try
            {
                if (mediaAsset.MediaAssetId == 0)
                {
                    MediaAssetTable.InsertOnSubmit(mediaAsset);
                }
                else
                {
                    MediaAssetTable.Context.Refresh(RefreshMode.KeepCurrentValues, mediaAsset);
                }

                MediaAssetTable.Context.SubmitChanges();
                
                return mediaAsset.MediaAssetId;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool DeleteMediaAsset(MediaAsset mediaAsset)
        {
            try
            {
                MediaAssetTable.DeleteOnSubmit(mediaAsset);
                MediaAssetTable.Context.SubmitChanges();
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public MediaAsset GetMediaAssetById(int mediaAssetId)
        {
            return MediaAssetTable.FirstOrDefault(x => x.MediaAssetId == mediaAssetId);
        }
    }
}
