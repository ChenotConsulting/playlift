using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel.Abstract;
using DomainModel.Entities;

namespace DomainModel.Concrete
{
    public class SqlUserPlaylistRepository : IUserPlaylistRepository
    {
        public Table<UserPlaylist> UserPlaylistTable;
        public SqlUserPlaylistRepository(string connString)
        {
            UserPlaylistTable = (new DataContext(connString)).GetTable<UserPlaylist>();
        }

        public IQueryable<UserPlaylist> UserPlaylist { get { return UserPlaylistTable; } }
        public bool SaveUserPlaylist(UserPlaylist userPlaylist)
        {
            try
            {
                if (userPlaylist.UserPlaylistId == 0)
                {
                    UserPlaylistTable.InsertOnSubmit(userPlaylist);
                }
                else
                {
                    UserPlaylistTable.Context.Refresh(RefreshMode.KeepCurrentValues, userPlaylist);
                }

                UserPlaylistTable.Context.SubmitChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteUserPlaylist(UserPlaylist userPlaylist)
        {
            try
            {
                UserPlaylistTable.DeleteOnSubmit(userPlaylist);
                UserPlaylistTable.Context.SubmitChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<UserPlaylist> GetUserPlaylistsByUserId(int userId)
        {
            return UserPlaylistTable.Where(x => x.UserId == userId).ToList();
        }

        public List<UserPlaylist> GetUserPlaylistsByPlaylistId(int playlistId)
        {
            return UserPlaylistTable.Where(x => x.PlaylistId == playlistId).ToList();
        }
    }
}
