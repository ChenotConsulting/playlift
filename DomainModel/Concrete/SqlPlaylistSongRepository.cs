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
    public class SqlPlaylistSongRepository : IPlaylistSongRepository
    {
        public Table<PlaylistSong> PlaylistSongTable;
        public SqlPlaylistSongRepository(string connString)
        {
            PlaylistSongTable = (new DataContext(connString)).GetTable<PlaylistSong>();
        }

        public IQueryable<PlaylistSong> PlaylistSong { get { return PlaylistSongTable; } }
        public bool SavePlaylistSong(PlaylistSong playlistSong)
        {
            try
            {
                if (playlistSong.PlaylistSongId == 0)
                {
                    PlaylistSongTable.InsertOnSubmit(playlistSong);
                }
                else
                {
                    PlaylistSongTable.Context.Refresh(RefreshMode.KeepCurrentValues, playlistSong);
                }

                PlaylistSongTable.Context.SubmitChanges();
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeletePlaylistSong(PlaylistSong playlistSong)
        {
            try
            {
                PlaylistSongTable.DeleteOnSubmit(playlistSong);
                PlaylistSongTable.Context.SubmitChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<PlaylistSong> GetSongsByPlaylistId(int playlistId)
        {
            return PlaylistSongTable.Where(x => x.PlaylistId == playlistId).ToList();
        }

        public List<PlaylistSong> GetPlaylistsBySongId(int songId)
        {
            return PlaylistSongTable.Where(x => x.SongId == songId).ToList();
        }

        public PlaylistSong GetPlaylistSongsBySongIdPlaylistId(int songId, int playlistId)
        {
            return PlaylistSongTable.FirstOrDefault(x => x.PlaylistSongId == playlistId && x.SongId == songId);
        }
    }
}
