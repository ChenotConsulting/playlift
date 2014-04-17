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
    public class SqlSongGenreRepository : ISongGenreRepository
    {
        public Table<SongGenre> SongGenreTable;
        public SqlSongGenreRepository(string connString)
        {
            SongGenreTable = (new DataContext(connString)).GetTable<SongGenre>();
        }

        public IQueryable<SongGenre> SongGenre { get { return SongGenreTable; } }
        public bool SaveSongGenre(SongGenre songGenre)
        {
            try
            {
                if (songGenre.SongGenreId == 0)
                {
                    SongGenreTable.InsertOnSubmit(songGenre);
                }
                else
                {
                    SongGenreTable.Context.Refresh(RefreshMode.KeepCurrentValues, songGenre);
                }

                SongGenreTable.Context.SubmitChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteSongGenre(SongGenre songGenre)
        {
            try
            {
                SongGenreTable.DeleteOnSubmit(songGenre);
                SongGenreTable.Context.SubmitChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<SongGenre> GetSongGenresBySongId(int songId)
        {
            return SongGenreTable.Where(x => x.SongId == songId).ToList();
        }

        public List<SongGenre> GetSongGenresByGenre(int genreId)
        {
            return SongGenreTable.Where(x => x.GenreId == genreId).ToList();
        }

        public List<SongGenre> GetSongGenresByGenres(List<Genre> genreCollection)
        {
            return genreCollection.Select(genre => SongGenreTable.FirstOrDefault(x => x.GenreId == genre.GenreId)).ToList();
        }
    }
}
