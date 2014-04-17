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
    public class SqlAlbumGenreRepository : IAlbumGenreRepository
    {
        public Table<AlbumGenre> AlbumGenreTable;
        public SqlAlbumGenreRepository(string connString)
        {
            AlbumGenreTable = (new DataContext(connString)).GetTable<AlbumGenre>();
        }

        public IQueryable<AlbumGenre> AlbumGenre { get { return AlbumGenreTable; } }
        public bool SaveAlbumGenre(AlbumGenre albumGenre)
        {
            try
            {
                if (albumGenre.AlbumGenreId == 0)
                {
                    AlbumGenreTable.InsertOnSubmit(albumGenre);
                }
                else
                {
                    AlbumGenreTable.Context.Refresh(RefreshMode.KeepCurrentValues, albumGenre);
                }

                AlbumGenreTable.Context.SubmitChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteAlbumGenre(AlbumGenre albumGenre)
        {
            try
            {
                AlbumGenreTable.DeleteOnSubmit(albumGenre);
                AlbumGenreTable.Context.SubmitChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteAllAlbumGenreByAlbum(int albumId)
        {
            var userGenreCollection = AlbumGenre.Where(x => x.AlbumId == albumId).ToList();

            try
            {
                foreach (var userGenre in userGenreCollection)
                {
                    AlbumGenreTable.DeleteOnSubmit(userGenre);
                    AlbumGenreTable.Context.SubmitChanges();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<AlbumGenre> GetAlbumGenresByAlbumId(int albumId)
        {
            return AlbumGenreTable.Where(x => x.AlbumId == albumId).ToList();
        }

        public List<AlbumGenre> GetAlbumGenresByGenreId(int genreId)
        {
            return AlbumGenreTable.Where(x => x.GenreId == genreId).ToList();
        }

        public List<AlbumGenre> GetAlbumGenresByGenres(List<Genre> genreCollection)
        {
            return genreCollection.Select(genre => AlbumGenreTable.FirstOrDefault(x => x.GenreId == genre.GenreId)).ToList();
        }
    }
}
