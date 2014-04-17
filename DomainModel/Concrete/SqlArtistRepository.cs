using DomainModel.Abstract;
using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Concrete
{
    public class SqlArtistRepository : IArtistRepository
    {
        private readonly Table<Artist> artistTable;
        public SqlArtistRepository(string connString)
        {
            artistTable = (new DataContext(connString)).GetTable<Artist>();
        }
        
        public IQueryable<Artist> Artist
        {
            get { return artistTable; }
        }

        public bool SaveArtist(Artist artist)
        {
            try
            {
                if (artist.ArtistId == 0)
                {
                    artistTable.InsertOnSubmit(artist);
                }
                else
                {
                    artistTable.Context.Refresh(RefreshMode.KeepCurrentValues, artist);
                }

                artistTable.Context.SubmitChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteArtist(Artist artist)
        {
            try
            {
                artistTable.DeleteOnSubmit(artist);
                artistTable.Context.SubmitChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Artist GetArtistByArtistId(int artistId)
        {
            return artistTable.FirstOrDefault(x => x.ArtistId == artistId);
        }

        public Artist GetArtistByUserId(int userId)
        {
            return artistTable.FirstOrDefault(x => x.UserId == userId);
        }
    }
}
