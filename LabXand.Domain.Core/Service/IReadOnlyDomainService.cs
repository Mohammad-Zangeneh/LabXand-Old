using LabXand.Core;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LabXand.DomainLayer.Core
{
    public interface IReadOnlyDomainService<TDomain, TIdentifier>
        where TDomain : DomainEntityBase<TIdentifier>
    {
        List<SortItem> DefaultSortItems { get; }        
        TDomain CreateNullObject();
        TDomain Find(Expression<Func<TDomain, bool>> criteria);
        IPagedList<TDomain> FindPage(Expression<Func<TDomain, bool>> criteria, List<SortItem> sortItems, int page, int size);
        IList<TDomain> FindAll(Expression<Func<TDomain, bool>> criteria, List<SortItem> sortItems);
        Task<TDomain> FindAsync(Expression<Func<TDomain, bool>> criteria);
        //Task<IPagedList<TDomain>> FindPageAsync(Expression<Func<TDomain, bool>> criteria, List<SortItem> sortItems, int page, int size);
        Task<IList<TDomain>> FindAllAsync(Expression<Func<TDomain, bool>> criteria, List<SortItem> sortItems);
        int Count(Expression<Func<TDomain, bool>> criteria);
        Task<int> CountAsync(Expression<Func<TDomain, bool>> criteria);
    }
}
