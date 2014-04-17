using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Abstract
{
    public interface IArtistEventRepository
    {
        IQueryable<ArtistEvent> ArtistEvent { get; }
        bool SaveEvent(ArtistEvent artistEvent);
        bool DeleteEvent(ArtistEvent artistEvent);
        ArtistEvent GetArtistEventByArtistEventId(int artistEventId);
        List<ArtistEvent> GetArtistEventsByEventId(int eventId);
        List<ArtistEvent> GetArtistEventsByArtistId(int artistId);
    }
}
