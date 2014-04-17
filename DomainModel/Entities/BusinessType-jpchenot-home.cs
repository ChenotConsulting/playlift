using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    [Table(Name="BusinessType")]
    public class BusinessType
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int BusinessTypeId { get; set; }
        [Column]
        public string BusinessTypeName { get; set; }
    }
}
