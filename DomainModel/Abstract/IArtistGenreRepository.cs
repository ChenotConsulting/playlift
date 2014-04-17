using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Abstract
{
    public interface IArtistGenreRepository
    {
        IQueryable<ArtistGenre> ArtistGenre { get; }
        bool SaveArtistGenre(ArtistGenre artistGenre);
        bool DeleteArtistGenre(ArtistGenre artistGenre);
        List<ArtistGenre> GetArtistGenresByArtistId(int userId);
        List<ArtistGenre> GetArtistGenresByGenreId(int genreId);
        List<ArtistGenre> GetArtistGenresByGenres(List<ArtistGenre> genreCollection);
    }
}
