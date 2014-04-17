using DomainModel.Abstract;
using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace DomainModel.Concrete
{
    public class SqlMediaAssetLocationRepository : IMediaAssetLocationRepository
    {
        public Table<MediaAssetLocation> MediaAssetLocationTable;
        public SqlMediaAssetLocationRepository(string connString)
        {
            MediaAssetLocationTable = (new DataContext(connString)).GetTable<MediaAssetLocation>();
        }

        public IQueryable<MediaAssetLocation> MediaAssetLocation { get { return MediaAssetLocationTable; } }
        public bool SaveMediaAssetLocation(MediaAssetLocation mediaAssetLocation)
        {
            try
            {
                if (mediaAssetLocation.MediaAssetLocationId == 0)
                {
                    MediaAssetLocationTable.InsertOnSubmit(mediaAssetLocation);
                }
                else
                {
                    MediaAssetLocationTable.Context.Refresh(RefreshMode.KeepCurrentValues, mediaAssetLocation);
                }

                MediaAssetLocationTable.Context.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteMediaAssetLocation(MediaAssetLocation mediaAssetLocation)
        {
            try
            {
                MediaAssetLocationTable.DeleteOnSubmit(mediaAssetLocation);
                MediaAssetLocationTable.Context.SubmitChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<MediaAssetLocation> GetMediaAssetLocations(int mediaAssetId)
        {
            return MediaAssetLocationTable.Where(x => x.MediaAssetId == mediaAssetId).ToList();
        }
    }
}
