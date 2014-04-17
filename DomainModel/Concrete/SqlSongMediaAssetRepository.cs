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
    public class SqlSongMediaAssetRepository : ISongMediaAssetRepository
    {
        public Table<SongMediaAsset> SongMediaAssetTable;
        public SqlSongMediaAssetRepository(string connString)
        {
            SongMediaAssetTable = (new DataContext(connString)).GetTable<SongMediaAsset>();
        }

        public IQueryable<SongMediaAsset> SongMediaAsset { get { return SongMediaAssetTable; } }
        public bool SaveSongMediaAsset(SongMediaAsset songMediaAsset)
        {
            try
            {
                if (songMediaAsset.SongMediaAssetId == 0)
                {
                    SongMediaAssetTable.InsertOnSubmit(songMediaAsset);
                }
                else
                {
                    SongMediaAssetTable.Context.Refresh(RefreshMode.KeepCurrentValues, songMediaAsset);
                }

                SongMediaAssetTable.Context.SubmitChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteSongMediaAsset(SongMediaAsset songMediaAsset)
        {
            try
            {
                SongMediaAssetTable.DeleteOnSubmit(songMediaAsset);
                SongMediaAssetTable.Context.SubmitChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<SongMediaAsset> GetSongMediaAssetsBySongId(int songId)
        {
            return SongMediaAssetTable.Where(x => x.SongId == songId).ToList();
        }

        public List<SongMediaAsset> GetSongMediaAssetsByMediaAssetId(int mediaAssetId)
        {
            return SongMediaAssetTable.Where(x => x.MediaAssetId == mediaAssetId).ToList();
        }
    }
}
