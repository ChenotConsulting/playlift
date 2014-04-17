
using System;using System.Collections.Generic;
using System.Linq;
using DomainModel.Entities;
using DomainModel.Abstract;
using System.Data.Linq;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Concrete
{
    public class SqlAccountRepository : IAccountRepository
    {
        private Table<Account> AccountTable;
        public SqlAccountRepository(string connString)
        {
            AccountTable = (new DataContext(connString)).GetTable<Account>();
        }

        public IQueryable<Account> Account
        {
            get { return AccountTable; }
        }

        public bool SaveAccount(Account account)
        {
            try
            {
                if (account.AccountId == 0)
                {
                    AccountTable.InsertOnSubmit(account);
                }
                else
                {
                    AccountTable.Context.Refresh(RefreshMode.KeepCurrentValues, account);
                }

                AccountTable.Context.SubmitChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteAccount(Account account)
        {
            try
            {
                AccountTable.DeleteOnSubmit(account);
                AccountTable.Context.SubmitChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
