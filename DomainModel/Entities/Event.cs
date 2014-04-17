using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    [Table(Name="Event")]
    public class Event
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int EventId { get; set; }
        [Column]
        public string Name { get; set; }
        [Column]
        public string Location { get; set; }
        [Column]
        public DateTime Date { get; set; }
        [Column]
        public string TicketInfo { get; set; }
        [Column]
        public string AdditionalInfo { get; set; }
        [Column]
        public DateTime CreatedDate { get; set; }
        [Column]
        public bool Active { get; set; }
    }
}
