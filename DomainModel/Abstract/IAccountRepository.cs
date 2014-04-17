using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Abstract
{
    public interface IAccountRepository
    {
        IQueryable<Account> Account { get; }
        bool SaveAccount(Account account);
        bool DeleteAccount(Account account);
    }
}
