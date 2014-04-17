using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Abstract
{
    public interface ISongMediaAssetRepository
    {
        IQueryable<SongMediaAsset> SongMediaAsset { get; }
        bool SaveSongMediaAsset(SongMediaAsset songMediaAsset);
        bool DeleteSongMediaAsset(SongMediaAsset songMediaAsset);
        List<SongMediaAsset> GetSongMediaAssetsBySongId(int songId);
        List<SongMediaAsset> GetSongMediaAssetsByMediaAssetId(int mediaAssetId);
    }
}
