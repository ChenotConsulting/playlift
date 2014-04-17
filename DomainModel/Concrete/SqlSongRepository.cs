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
    public class SqlSongRepository : ISongRepository
    {
        public Table<Song> SongTable;
        public SqlSongRepository(string connString)
        {
            SongTable = (new DataContext(connString)).GetTable<Song>();
        }

        public IQueryable<Song> Song { get { return SongTable; } }
        public int SaveSong(Song song)
        {
            try
            {
                if (song.SongId == 0)
                {
                    SongTable.InsertOnSubmit(song);
                }
                else
                {
                    SongTable.Context.Refresh(RefreshMode.KeepCurrentValues, song);
                }

                SongTable.Context.SubmitChanges();

                return song.SongId;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool DeleteSong(Song song)
        {
            try
            {
                SongTable.DeleteOnSubmit(song);
                SongTable.Context.SubmitChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Song> GetAllSongs()
        {
            return SongTable.ToList();
        }

        public List<Song> GetSongsByComposer(string composer)
        {
            return SongTable.Where(x => x.SongComposer == composer).ToList();
        }

        public List<Song> GetSongsByReleaseDate(DateTime releaseDateFrom, DateTime releaseDateTo)
        {
            return SongTable.Where(x => x.SongReleaseDate >= releaseDateFrom && x.SongReleaseDate <= releaseDateTo).ToList();
        }

        public Song GetSongById(int songId)
        {
            return SongTable.FirstOrDefault(x => x.SongId == songId);
        }
    }
}
