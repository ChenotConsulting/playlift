using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Abstract
{
    public interface IPlaylistSongRepository
    {
        IQueryable<PlaylistSong> PlaylistSong { get; }

        bool SavePlaylistSong(PlaylistSong playlistSong);
        bool DeletePlaylistSong(PlaylistSong playlistSong);
        List<PlaylistSong> GetSongsByPlaylistId(int playlistId);
        List<PlaylistSong> GetPlaylistsBySongId(int songId);
        PlaylistSong GetPlaylistSongsBySongIdPlaylistId(int songId, int playlistId);
    }
}
