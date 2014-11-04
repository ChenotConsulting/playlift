using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Abstract
{
    public interface IBusinessUserRepository
    {
        IQueryable<BusinessUser> BusinessUser { get; }
        bool SaveBusinessUser(BusinessUser businessUser);
        bool DeleteBusinessUser(BusinessUser businessUser);
        BusinessUser GetBusinessUserByUserId(int userId);
        BusinessUser GetBusinessUserByBusinessTypeId(int businessTypeId);
    }
}
