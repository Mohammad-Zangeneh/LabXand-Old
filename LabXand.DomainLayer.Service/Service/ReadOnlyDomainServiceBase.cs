using LabXand.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LabXand.Extensions;
using LabXand.DomainLayer.Core;
using System.Data.Entity;

namespace LabXand.DomainLayer
{
    public abstract class ReadOnlyDomainServiceBase<TDomain, TRepository, TIdentifier> : IReadOnlyDomainWithRepositoryService<TDomain, TRepository, TIdentifier>
        where TDomain : DomainEntityBase<TIdentifier>
        where TRepository : IRepository<TDomain, TIdentifier>
    {
        #region Properties
        public ReadOnlyDomainServiceBase(TRepository repository)
        {
            Repository = repository;
            DefaultSortItems = new List<SortItem>() { new SortItem() { SortFiledsSelector = "Id", Direction = SortDirection.Descending } };
        }
        public List<SortItem> DefaultSortItems { get; protected set; }
        public TRepository Repository { get; protected set; }
        #endregion

        #region Methodes
        public abstract TDomain CreateNullObject();

        public TDomain Find(IQueryable<TDomain> query)
        {
            TDomain result = query.FirstOrDefault();
            if (result != null)
                return result;
            return CreateNullObject();
        }

        public TDomain Find(Expression<Func<TDomain, bool>> criteria)
        {
            return Find(GetQuery(criteria));
        }

        protected IPagedList<TDomain> FindPage(IQueryable<TDomain> query, int page, int size, Criteria criteria, List<SortItem> sortItems)
        {
            return GenericFindPage<TDomain>(query ?? Repository.Set(), page, size, criteria != null ? ExpressionHelper.CreateFromCriteria<TDomain>(criteria) : null, sortItems);
        }

        protected IPagedList<TDomain> FindPage(IQueryable<TDomain> query, int page, int size, Expression<Func<TDomain, bool>> criteria, List<SortItem> sortItems)
        {
            return GenericFindPage<TDomain>(query ?? Repository.Set(), page, size, criteria, sortItems);
        }

        protected IPagedList<T> GenericFindPage<T>(IQueryable<T> query, int page, int size, Expression<Func<T, bool>> criteria = null, List<SortItem> sortItems = null)
            where T : class
        {
            return Repository.CreatePagedList<T>(GetGenericQuery<T>(query, criteria, sortItems), page, size);
        }

        protected IPagedList<T> GenericFindPage<T>(ICollection<T> list, int page, int size)
            where T : class
        {
            return Repository.CreatePagedList<T>(list, page, size);
        }

        public IPagedList<TDomain> FindPage(Expression<Func<TDomain, bool>> criteria, List<SortItem> sortItems, int page, int size)
        {
            return FindPage(null, page, size, criteria, sortItems);
        }

        protected IList<TDomain> FindAll(IQueryable<TDomain> query)
        {
            return query.ToList();
        }

        public IList<TDomain> FindAll(Expression<Func<TDomain, bool>> criteria, List<SortItem> sortItems)
        {
            return GetQuery(criteria, sortItems).ToList();
        }

        public int Count(Expression<Func<TDomain, bool>> criteria)
        {
            return GetQuery().Count(criteria);
        }

        protected IQueryable<TDomain> GetQuery(Expression<Func<TRepository, IQueryable<TDomain>>> querySelector, Expression<Func<TDomain, bool>> criteria = null, List<SortItem> sortItems = null)
        {
            Expression<Func<TRepository, IQueryable<TDomain>>> defaultRepositorySelector = r => r.Set();
            return GetGenericQuery<TDomain>(querySelector ?? defaultRepositorySelector, criteria, sortItems);
        }

        protected IQueryable<TDomain> GetQuery(Expression<Func<TDomain, bool>> criteria = null, List<SortItem> sortItems = null)
        {
            return GetQuery(null, criteria, sortItems);
        }

        protected IQueryable<T> GetGenericQuery<T>(IQueryable<T> query, Expression<Func<T, bool>> criteria = null, List<SortItem> sortItems = null)
            where T : class
        {
            if (criteria == null)
                criteria = (T t) => true;

            query = GetSortedQuery(query, sortItems);

            return query.Where(criteria);

        }

        protected IQueryable<T> GetGenericQuery<T>(Expression<Func<TRepository, IQueryable<T>>> querySelector, Expression<Func<T, bool>> criteria = null, List<SortItem> sortItems = null)
            where T : class
        {
            return GetGenericQuery<T>(querySelector.Compile().Invoke(Repository), criteria, sortItems);
        }

        protected IQueryable<T> GetSortedQuery<T>(IQueryable<T> query, List<SortItem> sortItems = null)
            where T : class
        {
            if (sortItems == null)
                sortItems = DefaultSortItems;

            if (sortItems != null)
                query = SortItemHelper.GetQueryable(sortItems, query);
            return query;
        }

        public async Task<TDomain> FindAsync(Expression<Func<TDomain, bool>> criteria)
        {
            var result = await GetQuery(criteria).FirstOrDefaultAsync();
            if (result != null)
                return result;
            return CreateNullObject();
        }

        public async Task<IList<TDomain>> FindAllAsync(Expression<Func<TDomain, bool>> criteria, List<SortItem> sortItems)
        {
            return await GetQuery(criteria, sortItems).ToListAsync();
        }

        public async Task<int> CountAsync(Expression<Func<TDomain, bool>> criteria)
        {
            return await GetQuery().CountAsync(criteria);
        }

        #endregion
    }
}
