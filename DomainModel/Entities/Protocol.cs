using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    [Table(Name="Protocol")]
    public class Protocol
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int ProtocolId { get; set; }
        [Column]
        public string ProtocolName { get; set; }
        [Column]
        public string ProtocolValue { get; set; }
    }
}
