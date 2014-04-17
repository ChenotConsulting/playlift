using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    [Table(Name="Payment")]
    public class Payment
    {
        public enum PaymentTypeList
        {
            Paypal = 1
        };

        public enum PaymentStatusList
        {
            Processing = 1,
            Cancelled = 2,
            Approved = 3,
            Rejected = 4
        };

        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int PaymentId { get; set; }
        [Column]
        public double PaymentTotal { get; set; }
        [Column]
        public int PaymentTypeId { get; set; }
        [Column]
        public int PaymentStatusId { get; set; }
        [Column]
        public DateTime PaymentDate { get; set; }
        [Column]
        public int AccountId { get; set; }

        public PaymentTypeList PaymentType { get; set; }
        public PaymentStatusList PaymentStatus { get; set; }
    }
}
