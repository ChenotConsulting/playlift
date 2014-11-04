using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    public class Business
    {
        public BusinessType BusinessType { get; set; }
        public List<Playlist> PlaylistCollection { get; set; }
        public BusinessUser BusinessUser { get; set; }
    }
}
