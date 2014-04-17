using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Abstract
{
    public interface IArtistRepository
    {
        IQueryable<Artist> Artist { get; }
        bool SaveArtist(Artist artist);
        bool DeleteArtist(Artist artist);
        Artist GetArtistByArtistId(int artistId);
        Artist GetArtistByUserId(int userId);
    }
}
