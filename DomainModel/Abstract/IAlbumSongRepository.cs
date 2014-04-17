using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Abstract
{
    public interface IAlbumSongRepository
    {
        IQueryable<AlbumSong> AlbumSong { get; }
        bool SaveAlbumSong(AlbumSong albumSong);
        bool DeleteAlbumSong(AlbumSong albumSong);
        List<AlbumSong> GetAlbumSongsByAlbumId(int albumId);
        List<AlbumSong> GetAlbumSongsBySongId(int songId);
    }
}
