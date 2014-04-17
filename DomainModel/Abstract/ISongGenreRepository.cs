using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Abstract
{
    public interface ISongGenreRepository
    {
        IQueryable<SongGenre> SongGenre { get; }
        bool SaveSongGenre(SongGenre songGenre);
        bool DeleteSongGenre(SongGenre songGenre);
        List<SongGenre> GetSongGenresBySongId(int songId);
        List<SongGenre> GetSongGenresByGenre(int genreId);
        List<SongGenre> GetSongGenresByGenres(List<Genre> genreCollection);
    }
}
