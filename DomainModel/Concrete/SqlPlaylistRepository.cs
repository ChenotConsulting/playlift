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
    public class SqlPlaylistRepository : IPlaylistRepository
    {
        private Table<Playlist> PlaylistTable;
        public SqlPlaylistRepository(string connString)
        {
            PlaylistTable = (new DataContext(connString)).GetTable<Playlist>();
        }

        public IQueryable<Playlist> Playlist { get { return PlaylistTable; } }
        public int SavePlaylist(Playlist playlist)
        {
            try
            {
                if (playlist.PlaylistId == 0)
                {
                    PlaylistTable.InsertOnSubmit(playlist);
                }
                else
                {
                    PlaylistTable.Context.Refresh(RefreshMode.KeepCurrentValues, playlist);
                }

                PlaylistTable.Context.SubmitChanges();

                return playlist.PlaylistId;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool DeletePlaylist(Playlist playlist)
        {
            try
            {
                PlaylistTable.DeleteOnSubmit(playlist);
                PlaylistTable.Context.SubmitChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Playlist> GetAllPlaylists()
        {
            return PlaylistTable.ToList();
        }

        public Playlist GetPlaylistById(int playlistId)
        {
            return PlaylistTable.FirstOrDefault(x => x.PlaylistId == playlistId);
        }
    }
}
