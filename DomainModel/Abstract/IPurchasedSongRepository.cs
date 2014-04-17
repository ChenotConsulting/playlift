using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Abstract
{
    public interface IPurchasedSongRepository
    {
        IQueryable<PurchasedSong> PurchasedSong { get; }

        bool SavePurchasedSong(PurchasedSong purchasedSong);
        bool DeletePurchasedSong(PurchasedSong purchasedSong);
        List<PurchasedSong> GetAllPurchasedSongs();
        List<PurchasedSong> GetPurchasedSongsByCustomerAccountId(int customerAccountId);
        List<PurchasedSong> GetPurchasedSongsByBusinessId(int businessId);
        List<PurchasedSong> GetPurchasedSongsByPlaylistSongId(int playlistSongId);
        PurchasedSong GetPurchasedSongById(int purchasedSongId);
    }
}
