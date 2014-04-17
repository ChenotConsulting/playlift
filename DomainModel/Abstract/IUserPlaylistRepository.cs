using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Abstract
{
    public interface IUserPlaylistRepository
    {
        IQueryable<UserPlaylist> UserPlaylist { get; }
        bool SaveUserPlaylist(UserPlaylist userPlaylist);
        bool DeleteUserPlaylist(UserPlaylist userPlaylist);
        List<UserPlaylist> GetUserPlaylistsByUserId(int userId);
        List<UserPlaylist> GetUserPlaylistsByPlaylistId(int playlistId);
    }
}
