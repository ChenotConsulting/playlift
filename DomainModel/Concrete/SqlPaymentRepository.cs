using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel.Abstract;
using DomainModel.Entities;

namespace DomainModel.Concrete
{
    public class SqlPaymentRepository : IPaymentRepository
    {
        public Table<Payment> PaymentTable;
        public SqlPaymentRepository(string connString)
        {
            PaymentTable = (new DataContext(connString)).GetTable<Payment>();
        }

        public IQueryable<Payment> Payment { get { return PaymentTable; } }
        public bool SavePayment(Payment payment)
        {
            try
            {
                if (payment.PaymentId == 0)
                {
                    PaymentTable.InsertOnSubmit(payment);
                }
                else
                {
                    PaymentTable.Context.Refresh(RefreshMode.KeepCurrentValues, payment);
                }

                PaymentTable.Context.SubmitChanges();
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeletePayment(Payment payment)
        {
            try
            {
                PaymentTable.DeleteOnSubmit(payment);
                PaymentTable.Context.SubmitChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Payment> GetAllPayments()
        {
            return PaymentTable.ToList();
        }

        public List<Payment> GetPaymentsByAccount(Account account)
        {
            return PaymentTable.Where(x => x.AccountId == account.AccountId).ToList();
        }

        public List<Payment> GetPaymentsByType(Payment.PaymentTypeList paymentType)
        {
            return PaymentTable.Where(x => x.PaymentTypeId == (int)paymentType).ToList();
        }

        public List<Payment> GetPaymentsByDate(DateTime paymentDateFrom, DateTime paymentDateTo)
        {
            return PaymentTable.Where(x => x.PaymentDate >= paymentDateFrom && x.PaymentDate <= paymentDateTo).ToList();
        }

        public List<Payment> GetPaymentsByStatus(Payment.PaymentStatusList paymentStatus)
        {
            return PaymentTable.Where(x => x.PaymentStatusId == (int)paymentStatus).ToList();
        }

        public Payment GetPaymentById(int paymentId)
        {
            return PaymentTable.FirstOrDefault(x => x.PaymentId == paymentId);
        }
    }
}
