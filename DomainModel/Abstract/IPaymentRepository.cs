using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Abstract
{
    public interface IPaymentRepository
    {
        IQueryable<Payment> Payment { get; }

        bool SavePayment(Payment payment);
        bool DeletePayment(Payment payment);
        List<Payment> GetAllPayments();
        List<Payment> GetPaymentsByAccount(Account account);
        List<Payment> GetPaymentsByType(Payment.PaymentTypeList paymentType);
        List<Payment> GetPaymentsByDate(DateTime paymentDateFrom, DateTime paymentDateTo);
        List<Payment> GetPaymentsByStatus(Payment.PaymentStatusList paymentStatus);
        Payment GetPaymentById(int paymentId);
    }
}
