using LabXand.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.DomainLayer.Core
{
    public interface IRepository<TDomain, TIdentifier>
        where TDomain : DomainEntityBase<TIdentifier>
    {
        List<IRowRestrictionSpecification<TDomain, TIdentifier>> RowRestrictionSpecifications { get; }
        IQueryable<TDomain> Set();
        void Add(TDomain domain);
        void Edit(TDomain domain);
        void Remove(TDomain domain);
        IPagedList<T> CreatePagedList<T>(IQueryable<T> query, int page, int size);
        IPagedList<T> CreatePagedList<T>(ICollection<T> list, int page, int size);
    }
}
