using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel.Entities;
using DomainModel.Abstract;
using System.Data.Linq;
using System.ComponentModel;

namespace DomainModel.Concrete
{
    public class SqlAlbumRepository : IAlbumRepository
    {
        private readonly Table<Album> albumTable;

        public SqlAlbumRepository(string connString)
        {
            albumTable = (new DataContext(connString)).GetTable<Album>();
        }

        public IQueryable<Album> Album
        {
            get { return albumTable; }
        }

        public int SaveAlbum(Album album)
        {
            try
            {
                if (album.AlbumId == 0)
                {
                    albumTable.InsertOnSubmit(album);
                }
                else
                {
                    albumTable.Context.Refresh(RefreshMode.KeepCurrentValues, album);
                }

                albumTable.Context.SubmitChanges();

                return album.AlbumId;
            }
            catch
            {
                return -1;
            }
        }

        public bool DeleteAlbum(Album album)
        {
            try
            {
                albumTable.DeleteOnSubmit(album);
                albumTable.Context.SubmitChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Album> GetAllAlbums()
        {
            return albumTable.ToList();
        }

        public List<Album> GetAllAlbumsByReleaseDate(DateTime releaseDateFrom, DateTime releaseDateTo)
        {
            return albumTable.Where(x => x.AlbumReleaseDate >= releaseDateFrom && x.AlbumReleaseDate <= releaseDateTo).ToList();
        }

        public List<Album> GetAllAlbumsByTitle(string title)
        {
            return albumTable.Where(x => x.AlbumTitle.Contains(title)).ToList();
        }

        public List<Album> GetAllAlbumsByProducer(string producer)
        {
            return albumTable.Where(x => x.AlbumTitle.Contains(producer)).ToList();
        }

        public List<Album> GetAllAlbumsByLabel(string label)
        {
            return albumTable.Where(x => x.AlbumTitle.Contains(label)).ToList();
        }

        public Album GetAlbumById(int albumId)
        {
            return albumTable.FirstOrDefault(x => x.AlbumId == albumId);
        }
    }
}
