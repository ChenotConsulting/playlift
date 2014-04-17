using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Abstract
{
    public interface IAlbumGenreRepository
    {
        IQueryable<AlbumGenre> AlbumGenre { get; }
        bool SaveAlbumGenre(AlbumGenre albumGenre);
        bool DeleteAlbumGenre(AlbumGenre albumGenre);
        List<AlbumGenre> GetAlbumGenresByAlbumId(int albumId);
        List<AlbumGenre> GetAlbumGenresByGenreId(int genreId);
        List<AlbumGenre> GetAlbumGenresByGenres(List<Genre> genreCollection);
    }
}
