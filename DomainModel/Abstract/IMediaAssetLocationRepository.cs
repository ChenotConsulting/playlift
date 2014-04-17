using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Abstract
{
    public interface IMediaAssetLocationRepository
    {
        IQueryable<MediaAssetLocation> MediaAssetLocation { get; }

        bool SaveMediaAssetLocation(MediaAssetLocation mediaAssetLocation);
        bool DeleteMediaAssetLocation(MediaAssetLocation mediaAssetLocation);
        List<MediaAssetLocation> GetMediaAssetLocations(int mediaAssetId);
    }
}
