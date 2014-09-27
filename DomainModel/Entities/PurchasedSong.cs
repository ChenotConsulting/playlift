using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    [Table(Name="PurchasedSong")]
    public class PurchasedSong
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int PurchasedSongId { get; set; }
        [Column]
        public int PlaylistSongId { get; set; }
        [Column]
        public double Cost { get; set; }
        [Column]
        public DateTime DatePurchased { get; set; }
        [Column]
        public int UserId { get; set; }

        public Business Business { get; set; }
    }
}
