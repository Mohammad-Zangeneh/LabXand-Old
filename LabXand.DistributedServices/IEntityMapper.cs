using LabXand.DomainLayer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.DistributedServices
{
    public interface IEntityMapper<TSource, TDestination>
    {
        TDestination MapTo(TSource source);
        TSource CreateFrom(TDestination destination);
    }
}
