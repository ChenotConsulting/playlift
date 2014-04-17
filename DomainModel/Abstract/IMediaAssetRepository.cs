using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Abstract
{
    public interface IMediaAssetRepository
    {
        IQueryable<MediaAsset> MediaAsset { get; }

        int SaveMediaAsset(MediaAsset mediaAsset);
        bool DeleteMediaAsset(MediaAsset mediaAsset);
        MediaAsset GetMediaAssetById(int mediaAssetId);
    }
}
