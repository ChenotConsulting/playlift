using DomainModel.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel.Entities;

namespace DomainModel.Concrete
{
    public class SqlAlbumSongRepository : IAlbumSongRepository
    {
        public Table<AlbumSong> AlbumSongTable;
        public SqlAlbumSongRepository(string connString)
        {
            AlbumSongTable = (new DataContext(connString)).GetTable<AlbumSong>();
        }

        public IQueryable<AlbumSong> AlbumSong
        {
            get { return AlbumSongTable; }
        }

        public bool SaveAlbumSong(AlbumSong albumSong)
        {
            try
            {
                if (albumSong.AlbumSongId == 0)
                {
                    AlbumSongTable.InsertOnSubmit(albumSong);
                }
                else
                {
                    AlbumSongTable.Context.Refresh(RefreshMode.KeepCurrentValues, albumSong);
                }

                AlbumSongTable.Context.SubmitChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteAlbumSong(AlbumSong albumSong)
        {
            try
            {
                AlbumSongTable.DeleteOnSubmit(albumSong);
                AlbumSongTable.Context.SubmitChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<AlbumSong> GetAlbumSongsByAlbumId(int albumId)
        {
            return AlbumSongTable.Where(x => x.AlbumId == albumId).ToList();
        }

        public List<AlbumSong> GetAlbumSongsBySongId(int songId)
        {
            return AlbumSongTable.Where(x => x.SongId == songId).ToList();
        }
    }
}
