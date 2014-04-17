using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel.Entities;

namespace DomainModel.Abstract
{
    public interface IArtistAlbumRepository
    {
        IQueryable<ArtistAlbum> ArtistAlbum { get; }
        bool SaveArtistAlbum(ArtistAlbum userAlbum);
        bool DeleteArtistAlbum(ArtistAlbum userAlbum);
        List<ArtistAlbum> GetArtistAlbumsByArtistId(int userId);
        List<ArtistAlbum> GetArtistAlbumsByAlbumId(int albumId);
    }
}
