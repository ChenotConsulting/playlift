using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Concrete
{
    public class SqlArtistEventRepository
    {
        private readonly Table<ArtistEvent> artistEventTable;
        public SqlArtistEventRepository(string connString)
        {
            artistEventTable = (new DataContext(connString)).GetTable<ArtistEvent>();
        }

        public IQueryable<Entities.ArtistEvent> ArtistEvent
        {
            get { return artistEventTable; }
        }

        public bool SaveArtistEvent(Entities.ArtistEvent artistEvent)
        {
            try
            {
                if (artistEvent.ArtistEventId == 0)
                {
                    artistEventTable.InsertOnSubmit(artistEvent);
                }
                else
                {
                    artistEventTable.Context.Refresh(RefreshMode.KeepCurrentValues, artistEvent);
                }

                artistEventTable.Context.SubmitChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteArtistEvent(Entities.ArtistEvent artistEvent)
        {
            try
            {
                artistEventTable.DeleteOnSubmit(artistEvent);
                artistEventTable.Context.SubmitChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ArtistEvent GetArtistEventByArtistEventId(int artistEventId)
        {
            return artistEventTable.FirstOrDefault(x => x.ArtistEventId == artistEventId);
        }

        public List<Entities.ArtistEvent> GetArtistEventsByEventId(int eventId)
        {
            return artistEventTable.Where(x => x.EventId == eventId).ToList();
        }

        public List<Entities.ArtistEvent> GetArtistEventsByArtistId(int artistId)
        {
            return artistEventTable.Where(x => x.ArtistId == artistId).ToList();
        }
    }
}
