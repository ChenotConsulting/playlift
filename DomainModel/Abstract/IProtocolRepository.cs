using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Abstract
{
    public interface IProtocolRepository
    {
        IQueryable<Protocol> Protocol { get; }
        Protocol GetProtocolById(int protocolId);
    }
}
