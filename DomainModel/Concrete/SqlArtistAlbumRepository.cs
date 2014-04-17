using DomainModel.Entities;
using DomainModel.Abstract;
using System.Data.Linq;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Concrete
{
    public class SqlArtistAlbumRepository : IArtistAlbumRepository
    {
        private readonly Table<ArtistAlbum> artistAlbumTable;
        public SqlArtistAlbumRepository(string connString)
        {
            artistAlbumTable = (new DataContext(connString)).GetTable<ArtistAlbum>();
        }

        public IQueryable<ArtistAlbum> ArtistAlbum
        {
            get { return artistAlbumTable; }
        }

        public bool SaveArtistAlbum(ArtistAlbum userAlbum)
        {
            try
            {
                if (userAlbum.ArtistAlbumId == 0)
                {
                    artistAlbumTable.InsertOnSubmit(userAlbum);
                }
                else
                {
                    artistAlbumTable.Context.Refresh(RefreshMode.KeepCurrentValues, userAlbum);
                }

                artistAlbumTable.Context.SubmitChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteArtistAlbum(Entities.ArtistAlbum userAlbum)
        {
            try
            {
                artistAlbumTable.DeleteOnSubmit(userAlbum);
                artistAlbumTable.Context.SubmitChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<ArtistAlbum> GetArtistAlbumsByArtistId(int artistId)
        {
            return artistAlbumTable.Where(x => x.UserId == artistId).ToList();
        }

        public List<ArtistAlbum> GetArtistAlbumsByAlbumId(int albumId)
        {
            return artistAlbumTable.Where(x => x.AlbumId == albumId).ToList();
        }
    }
}
