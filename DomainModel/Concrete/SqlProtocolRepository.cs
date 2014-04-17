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
    public class SqlProtocolRepository : IProtocolRepository
    {
        public Table<Protocol> ProtocolTable;
        public SqlProtocolRepository(string connString)
        {
            ProtocolTable = (new DataContext(connString)).GetTable<Protocol>();
        }

        public IQueryable<Protocol> Protocol { get { return ProtocolTable; } }

        public Protocol GetProtocolById(int protocolId)
        {
            return ProtocolTable.FirstOrDefault(x => x.ProtocolId == protocolId);
        }
    }
}
