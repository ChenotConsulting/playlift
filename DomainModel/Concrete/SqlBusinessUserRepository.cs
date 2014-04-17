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
    public class SqlBusinessUserRepository : IBusinessUserRepository
    {
        public Table<BusinessUser> BusinessUserTable;
        public SqlBusinessUserRepository(string connString)
        {
            BusinessUserTable = (new DataContext(connString)).GetTable<BusinessUser>();
        }

        public IQueryable<BusinessUser> BusinessUser { get { return BusinessUserTable; } }
        public bool SaveBusinessUser(BusinessUser businessUser)
        {
            try
            {
                if (businessUser.BusinessUserId == 0)
                {
                    BusinessUserTable.InsertOnSubmit(businessUser);
                }
                else
                {
                    BusinessUserTable.Context.Refresh(RefreshMode.KeepCurrentValues, businessUser);
                }

                BusinessUserTable.Context.SubmitChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteBusinessUser(BusinessUser businessUser)
        {
            try
            {
                BusinessUserTable.DeleteOnSubmit(businessUser);
                BusinessUserTable.Context.SubmitChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public BusinessUser GetBusinessUserByUserId(int userId)
        {
            return BusinessUserTable.FirstOrDefault(x => x.UserId == userId);
        }
    }
}
