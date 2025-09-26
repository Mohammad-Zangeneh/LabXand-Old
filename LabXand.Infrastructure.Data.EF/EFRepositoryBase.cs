using LabXand.Core;
using LabXand.DomainLayer;
using LabXand.DomainLayer.Core;
using LabXand.Extensions;
using LabXand.Infrastructure.Data.EF;
using LabXand.Infrastructure.Data.EF.UpdateConfiguration;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Infrastructure.Data.EF
{
    public abstract class EFRepositoryBase<TDomain, TIdentifier> : IRepository<TDomain, TIdentifier>
        where TDomain : DomainEntityBase<TIdentifier>
    {
        public EFRepositoryBase(IDataContext dataContext)
        {
            DataContext = dataContext as DbContextBase;
            ConstantFields = new List<string>();
            IncludeNavigationProperties = new List<string>();
            NavigationPropertiesMustbeUpdate = new List<string>();
            RowRestrictionSpecifications = new List<IRowRestrictionSpecification<TDomain, TIdentifier>>();
            UpdateConfiguration = new UpdateRootConfiguration<TDomain>(ConstantFields);
        }

        protected DbContextBase DataContext { get; set; }

        public List<IRowRestrictionSpecification<TDomain, TIdentifier>> RowRestrictionSpecifications { get; private set; }

        protected IUpdateConfiguration<TDomain> UpdateConfiguration { get; set; }

        protected virtual List<string> ConstantFields { get; private set; }

        protected List<string> IncludeNavigationProperties { get; set; }

        protected List<string> NavigationPropertiesMustbeUpdate { get; set; }

        private IDbSet<TDomain> _entities;
        protected IDbSet<TDomain> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = DataContext.Entities<TDomain>();
                return _entities;
            }
        }
        public IQueryable<TDomain> Set(List<string> navigationProperties)
        {
            if (RowRestrictionSpecifications != null && RowRestrictionSpecifications.Count > 0)
            {
                ParameterExpression parameter = Expression.Parameter(typeof(TDomain), "domainEntity");
                Expression restrictionExpression = null;
                foreach (var item in RowRestrictionSpecifications)
                {
                    if (restrictionExpression == null)
                        restrictionExpression = ExpressionHelper.Rewrite(item.Get(), parameter).Body;
                    else
                        restrictionExpression = Expression.And(restrictionExpression, ExpressionHelper.Rewrite(item.Get(), parameter).Body);
                }
                if (restrictionExpression != null)
                    return DataContext.EntitySet<TDomain>(navigationProperties).Where(Expression.Lambda<Func<TDomain, bool>>(restrictionExpression, parameter));
            }
            return DataContext.EntitySet<TDomain>(navigationProperties);
        }

        public IQueryable<TDomain> Set()
        {
            return this.Set(IncludeNavigationProperties);
        }

        public IPagedList<T> CreatePagedList<T>(IQueryable<T> query, int page, int size)
        {
            return new PagedList<T>(query, page, size);
        }

        public IPagedList<T> CreatePagedList<T>(ICollection<T> list, int page, int size)
        {
            return new PagedList<T>(list, page, size);
        }

        public virtual void Add(TDomain domain)
        {
            OnBeforeAdd(domain);
            DataContext.Add(domain);
            OnAfterAdd(domain);
        }

        public virtual void Edit(TDomain domain)
        {
            TDomain originalEntity = DataContext.EntitySet<TDomain>(NavigationPropertiesMustbeUpdate, false).SingleOrDefault(ExpressionHelper.CreateEqualCondition<TDomain, TIdentifier>("Id", domain.Id));
            OnBeforeEdit(domain, originalEntity);
            DataContext.UpdateDetachedObject<TDomain, TIdentifier>(domain, originalEntity, UpdateConfiguration);
            OnAfterEdit(domain, originalEntity);
        }

        public virtual void Remove(TDomain domain)
        {
            OnBeforeRemove(domain);
            DataContext.Remove<TDomain>(domain);
            OnAfterRemove(domain);
        }

        protected virtual void OnBeforeAdd(TDomain domain) { }
        protected virtual void OnAfterAdd(TDomain domain) { }
        protected virtual void OnBeforeEdit(TDomain domain, TDomain originalEntity) { }
        protected virtual void OnAfterEdit(TDomain domain, TDomain originalEntity) { }
        protected virtual void OnBeforeRemove(TDomain domain) { }
        protected virtual void OnAfterRemove(TDomain domain) { }


        #region Extenstions
        public IQueryable<TDomain> Set(List<Expression<Func<TDomain, dynamic>>> navigationProperties)
        {
            return Set(navigationProperties.Select(n => ExpressionHelper.GetNameOfProperty(n)).ToList());
        }

        public EFRepositoryBase<TDomain, TIdentifier> HasNavigationProperty(List<Expression<Func<TDomain, dynamic>>> navigationProperties)
        {
            IncludeNavigationProperties.AddRange(navigationProperties.Select(n => ExpressionHelper.GetNameOfProperty(n)));
            return this;
        }

        public EFRepositoryBase<TDomain, TIdentifier> HasUpdateNavigationProperty(List<Expression<Func<TDomain, dynamic>>> navigationProperties)
        {
            NavigationPropertiesMustbeUpdate.AddRange(navigationProperties.Select(n => ExpressionHelper.GetNameOfProperty(n)));
            return this;
        }

        public EFRepositoryBase<TDomain, TIdentifier> HasNavigationProperty(List<string> navigationProperties)
        {
            IncludeNavigationProperties.AddRange(navigationProperties);
            return this;
        }
        public EFRepositoryBase<TDomain, TIdentifier> HasUpdateNavigationProperty(List<string> navigationProperties)
        {
            NavigationPropertiesMustbeUpdate.AddRange(navigationProperties);
            return this;
        }
        public EFRepositoryBase<TDomain, TIdentifier> HasRestriction(IRowRestrictionSpecification<TDomain, TIdentifier> restriction)
        {
            RowRestrictionSpecifications.Add(restriction);
            return this;
        }
        #endregion
    }
}
