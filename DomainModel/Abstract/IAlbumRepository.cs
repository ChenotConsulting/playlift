using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Abstract
{
    interface IAlbumRepository
    {
        IQueryable<Album> Album { get; }

        int SaveAlbum(Album album);
        bool DeleteAlbum(Album album);
        List<Album> GetAllAlbums();
        List<Album> GetAllAlbumsByReleaseDate(DateTime releaseDateFrom, DateTime releaseDateTo);
        List<Album> GetAllAlbumsByTitle(string title);
        List<Album> GetAllAlbumsByProducer(string producer);
        List<Album> GetAllAlbumsByLabel(string label);
        Album GetAlbumById(int albumId);
    }
}
