using DomainModel.Entities;
using System.Linq;

namespace DomainModel.Abstract
{
    public interface IEventRepository
    {
        IQueryable<Event> Event { get; }
        bool SaveEvent(Event singleEvent);
        bool DeleteEvent(Event singleEvent);
        Event GetEventByEventId(int eventId);
    }
}
