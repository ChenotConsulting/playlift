using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Abstract
{
    public interface ISongRepository
    {
        IQueryable<Song> Song { get; }

        int SaveSong(Song song);
        bool DeleteSong(Song song);
        List<Song> GetAllSongs();
        List<Song> GetSongsByComposer(string composer);
        List<Song> GetSongsByReleaseDate(DateTime releaseDateFrom, DateTime releaseDateTo);
        Song GetSongById(int songId);
    }
}
