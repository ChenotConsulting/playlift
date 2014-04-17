using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Abstract
{
    public interface IPlaylistRepository
    {
        IQueryable<Playlist> Playlist { get; }

        int SavePlaylist(Playlist playlist);
        bool DeletePlaylist(Playlist playlist);
        List<Playlist> GetAllPlaylists();
        Playlist GetPlaylistById(int playlistId);
    }
}
