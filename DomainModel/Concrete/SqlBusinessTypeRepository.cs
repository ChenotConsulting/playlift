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
    public class SqlBusinessTypeRepository : IBusinessTypeRepository
    {
        public Table<BusinessType> BusinessTypeTable;
        public SqlBusinessTypeRepository(string connString)
        {
            BusinessTypeTable = (new DataContext(connString)).GetTable<BusinessType>();
        }

        public IQueryable<BusinessType> BusinessType { get { return BusinessTypeTable; } }

        public BusinessType GetBusinessTypeById(int businessTypeId)
        {
            return BusinessTypeTable.FirstOrDefault(x => x.BusinessTypeId == businessTypeId);
        }
    }
}
