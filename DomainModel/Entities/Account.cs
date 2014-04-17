using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    [Table(Name="Account")]
    public class Account
    {
        public enum AccountTypeList
        {
            admin = 1,
            customer = 2,
            artist = 3,
            business = 4
        }

        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int AccountId { get; set; }
        [Column]
        public int UserId { get; set; }
        [Column]
        public bool Active { get; set; }
        [Column]
        public int Credits { get; set; }
        [Column]
        public DateTime CreatedDate { get; set; }
        [Column]
        public int AccountTypeId { get; set; }

        public User User { get; set; }
        public AccountTypeList AccountType { get; set; }
        public List<PurchasedSong> PurchasedSongCollection { get; set; }
        public List<Payment> PaymentCollection { get; set; }
    }
}
