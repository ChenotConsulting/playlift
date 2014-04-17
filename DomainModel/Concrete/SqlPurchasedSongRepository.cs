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
    public class SqlPurchasedSongRepository : IPurchasedSongRepository
    {
        public Table<PurchasedSong> PurchasedSongTable;
        public SqlPurchasedSongRepository(string connString)
        {
            PurchasedSongTable = (new DataContext(connString)).GetTable<PurchasedSong>();
        }

        public IQueryable<PurchasedSong> PurchasedSong { get { return PurchasedSongTable; } }
        public bool SavePurchasedSong(PurchasedSong purchasedSong)
        {
            try
            {
                if (purchasedSong.PurchasedSongId == 0)
                {
                    PurchasedSongTable.InsertOnSubmit(purchasedSong);
                }
                else
                {
                    PurchasedSongTable.Context.Refresh(RefreshMode.KeepCurrentValues, purchasedSong);
                }

                PurchasedSongTable.Context.SubmitChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeletePurchasedSong(PurchasedSong purchasedSong)
        {
            try
            {
                PurchasedSongTable.DeleteOnSubmit(purchasedSong);
                PurchasedSongTable.Context.SubmitChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<PurchasedSong> GetAllPurchasedSongs()
        {
            return PurchasedSongTable.ToList();
        }

        public List<PurchasedSong> GetPurchasedSongsByCustomerAccountId(int customerAccountId)
        {
            return PurchasedSongTable.Where(x => x.CustmomerAccountId == customerAccountId).ToList();
        }

        public List<PurchasedSong> GetPurchasedSongsByBusinessId(int businessId)
        {
            return PurchasedSongTable.Where(x => x.BusinessId == businessId).ToList();
        }

        public List<PurchasedSong> GetPurchasedSongsByPlaylistSongId(int playlistSongId)
        {
            return PurchasedSongTable.Where(x => x.PlaylistSongId == playlistSongId).ToList();
        }

        public PurchasedSong GetPurchasedSongById(int purchasedSongId)
        {
            return PurchasedSongTable.FirstOrDefault(x => x.PurchasedSongId == purchasedSongId);
        }
    }
}
