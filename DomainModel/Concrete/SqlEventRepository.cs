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
    public class SqlEventRepository : IEventRepository
    {
        private readonly Table<Event> eventTable;
        public SqlEventRepository(string connString)
        {
            eventTable = (new DataContext(connString)).GetTable<Event>();
        }

        public IQueryable<Entities.Event> Event
        {
            get { return eventTable; }
        }

        public bool SaveEvent(Entities.Event singleEvent)
        {
            try
            {
                if (singleEvent.EventId == 0)
                {
                    eventTable.InsertOnSubmit(singleEvent);
                }
                else
                {
                    eventTable.Context.Refresh(RefreshMode.KeepCurrentValues, singleEvent);
                }

                eventTable.Context.SubmitChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteEvent(Entities.Event singleEvent)
        {
            try
            {
                eventTable.DeleteOnSubmit(singleEvent);
                eventTable.Context.SubmitChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Entities.Event GetEventByEventId(int eventId)
        {
            return eventTable.FirstOrDefault(x => x.EventId == eventId);
        }
    }
}
