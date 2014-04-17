using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel.Abstract;
using DomainModel.Entities;
using TRMInfrastructure.Utilities;

namespace DomainModel.Concrete
{
    public class SqlArtistGenreRepository : IArtistGenreRepository
    {
        private Utilities util = new Utilities();
        public Table<ArtistGenre> UserGenreTable;
        public SqlArtistGenreRepository(string connString)
        {
            UserGenreTable = (new DataContext(connString)).GetTable<ArtistGenre>();
        }

        public IQueryable<ArtistGenre> ArtistGenre { get { return UserGenreTable; } }
        public bool SaveArtistGenre(ArtistGenre userGenre)
        {
            try
            {
                if (userGenre.ArtistGenreId == 0)
                {
                    UserGenreTable.InsertOnSubmit(userGenre);
                }
                else
                {
                    UserGenreTable.Context.Refresh(RefreshMode.KeepCurrentValues, userGenre);
                }

                UserGenreTable.Context.SubmitChanges();

                return true;
            }
            catch(Exception ex)
            {
                util.ErrorNotification(ex);
                throw;
            }
        }

        public bool DeleteArtistGenre(ArtistGenre userGenre)
        {
            try
            {
                UserGenreTable.DeleteOnSubmit(userGenre);
                UserGenreTable.Context.SubmitChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteAllArtistGenreByArtist(int userId)
        {
            var userGenreCollection = ArtistGenre.Where(x => x.UserId == userId).ToList();

            try
            {
                foreach (var userGenre in userGenreCollection)
                {
                    UserGenreTable.DeleteOnSubmit(userGenre);
                    UserGenreTable.Context.SubmitChanges();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<ArtistGenre> GetArtistGenresByArtistId(int userId)
        {
            return UserGenreTable.Where(x => x.UserId == userId).ToList();
        }

        public List<ArtistGenre> GetArtistGenresByGenreId(int genreId)
        {
            return UserGenreTable.Where(x => x.GenreId == genreId).ToList();
        }

        public List<ArtistGenre> GetArtistGenresByGenres(List<ArtistGenre> genreCollection)
        {
            return genreCollection.Select(genre => UserGenreTable.FirstOrDefault(x => x.GenreId == genre.GenreId)).ToList();
        }
    }
}
