using DomainModel.Abstract;
using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Concrete
{
    public class SqlCountyCityRepository : ICountyCityRepository
    {
        private readonly Table<CountyCity> countyCityTable;
        public SqlCountyCityRepository(string connString)
        {
            countyCityTable = (new DataContext(connString)).GetTable<CountyCity>();
        }

        public IQueryable<CountyCity> CountyCity
        {
            get { return countyCityTable; }
        }
    }
}
