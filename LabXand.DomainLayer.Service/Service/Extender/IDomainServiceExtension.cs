using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LabXand.Core;
using LabXand.DomainLayer.Core;
using LabXand.Extensions;

namespace LabXand.DomainLayer
{
    public static class IDomainServiceExtension
    {
        #region Validations
        #region Single Value Field Uniqueness
        public static IDomainService<TDomain, TIdentifier> HasUniqueFields<TDomain, TIdentifier>(
            this IDomainService<TDomain, TIdentifier> domainService,
            List<string> uniqueFields,
            string identifierPropertyName = "",
            Func<Expression<Func<TDomain, bool>>, int> countMethod = null)
                where TDomain : DomainEntityBase<TIdentifier>
        {
            if (countMethod == null)
                countMethod = domainService.Count;
            var currentValidatior = domainService.OnSaveValidators.OfType<SingleValueFieldMustBeUnique<TDomain, TIdentifier>>().FirstOrDefault();

            if (currentValidatior == null)
                currentValidatior = string.IsNullOrWhiteSpace(identifierPropertyName) ? new SingleValueFieldMustBeUnique<TDomain, TIdentifier>(domainService, countMethod) : new SingleValueFieldMustBeUnique<TDomain, TIdentifier>(domainService, identifierPropertyName, countMethod);

            if (currentValidatior.FieldSpecifications == null)
                currentValidatior.FieldSpecifications = new List<List<string>>();
            currentValidatior.FieldSpecifications.Add(uniqueFields);
            domainService.OnSaveValidators.Add(currentValidatior);
            return domainService;
        }

        public static IDomainService<TDomain, TIdentifier> HasUniqueFields<TDomain, TIdentifier>(
            this IDomainService<TDomain, TIdentifier> domainService,
            List<Expression<Func<TDomain, dynamic>>> uniqueFields,
            string identifierPropertyName = "")
                where TDomain : DomainEntityBase<TIdentifier>
        {
            return HasUniqueFields(domainService, uniqueFields.Select(u => ExpressionHelper.GetNameOfProperty(u)).ToList(), identifierPropertyName);
        }

        public static IDomainService<TDomain, TIdentifier> HasUniqueFields<TDomain, TIdentifier>(
            this IDomainService<TDomain, TIdentifier> domainService,
            List<Expression<Func<TDomain, dynamic>>> uniqueFields,
            Expression<Func<TDomain, dynamic>> identifierPropertyName = null)
                where TDomain : DomainEntityBase<TIdentifier>
        {
            return HasUniqueFields(domainService, uniqueFields.Select(u => ExpressionHelper.GetNameOfProperty(u)).ToList(), identifierPropertyName == null ? string.Empty : ExpressionHelper.GetNameOfProperty(identifierPropertyName));
        }
        #endregion

        #region Collection Value Field Uniqueness
        public static IDomainService<TDomain, TIdentifier> HasCollectionUniqueFields<TDomain, TProperty, TIdentifier>(
            this IDomainService<TDomain, TIdentifier> domainService,
            List<string> uniqueFields,
            Func<TDomain, IEnumerable<TProperty>> collectionPropertySelector,
            string identifierPropertyName = "")
                where TDomain : DomainEntityBase<TIdentifier>
        {
            var currentValidatior = domainService.OnSaveValidators.OfType<CollectionValueFieldMustBeUnique<TDomain, TProperty, TIdentifier>>().FirstOrDefault();

            if (currentValidatior == null)
                currentValidatior = string.IsNullOrWhiteSpace(identifierPropertyName) ? new CollectionValueFieldMustBeUnique<TDomain, TProperty, TIdentifier>(domainService, collectionPropertySelector) :
                    new CollectionValueFieldMustBeUnique<TDomain, TProperty, TIdentifier>(domainService, identifierPropertyName, collectionPropertySelector);

            currentValidatior.FieldSpecifications = uniqueFields;
            domainService.OnSaveValidators.Add(currentValidatior);
            return domainService;
        }

        public static IDomainService<TDomain, TIdentifier> HasCollectionUniqueFields<TDomain, TProperty, TIdentifier>(
            this IDomainService<TDomain, TIdentifier> domainService,
            List<Expression<Func<TDomain, dynamic>>> uniqueFields,
            Func<TDomain, IEnumerable<TProperty>> collectionPropertySelector,
            string identifierPropertyName = "")
                where TDomain : DomainEntityBase<TIdentifier>
        {
            return HasCollectionUniqueFields(domainService, uniqueFields.Select(u => ExpressionHelper.GetNameOfProperty(u)).ToList(), collectionPropertySelector, identifierPropertyName);
        }
        #endregion
        #endregion

        #region Criteria
        public static Criteria GetCriteria<TDomain, TIdentifier>(
            this IDomainService<TDomain, TIdentifier> domainService)
            where TDomain : DomainEntityBase<TIdentifier>
        {
            return CriteriaBuilder.CreateNew<TDomain>();
        }
        #endregion

        #region Remove

        public static void Remove<TDomain, TIdentifier>(
            this IDomainService<TDomain, TIdentifier> domainService,
            Expression<Func<TDomain, bool>> predicate)
            where TDomain : DomainEntityBase<TIdentifier>
        {
            IList<TDomain> entities = domainService.FindAll(predicate).ToList();
            domainService.Remove(entities);
        }

        public static void Remove<TDomain, TIdentifier>(
            this IDomainService<TDomain, TIdentifier> domainService,
            Expression<Func<TDomain, bool>> predicate,
            IEnumerable<TIdentifier> identifiers)
            where TDomain : DomainEntityBase<TIdentifier>
        {
            domainService.Remove(e => identifiers.Contains(e.Id));
        }

        public static void Remove<TDomain, TIdentifier>(
            this IDomainService<TDomain, TIdentifier> domainService,
            TIdentifier id)
                where TDomain : DomainEntityBase<TIdentifier>
        {
            TDomain entity = domainService.Find(id);
            if (entity != null) domainService.Remove(entity);
        }

        public static void Remove<TDomain, TIdentifier>(
            this IDomainService<TDomain, TIdentifier> domainService,
            IList<TDomain> entities)
                where TDomain : DomainEntityBase<TIdentifier>
        {
            for (int i = 0; i < entities.Count - 1; i++)
                domainService.Remove(entities[i]);
            domainService.Remove(entities[entities.Count - 1]);
        }

        #endregion

        #region Find All
        public static IList<TDomain> FindAll<TDomain, TIdentifier>(
            this IDomainService<TDomain, TIdentifier> domainService, Expression<Func<TDomain, bool>> criteria)
            where TDomain : DomainEntityBase<TIdentifier>
        {
            return domainService.FindAll(criteria, domainService.DefaultSortItems);
        }

        public static IList<TDomain> FindAll<TDomain, TIdentifier>(
            this IDomainService<TDomain, TIdentifier> domainService)
            where TDomain : DomainEntityBase<TIdentifier>
        {
            return domainService.FindAll(null, null);
        }

        public static IList<TDomain> FindAll<TDomain, TIdentifier>(
            this IDomainService<TDomain, TIdentifier> domainService,
            IEnumerable<TIdentifier> ids)
                where TDomain : DomainEntityBase<TIdentifier>
        {
            return domainService.FindAll(e => ids.Contains(e.Id), domainService.DefaultSortItems);
        }

        public static IList<TDomain> FindAll<TDomain, TIdentifier>(
            this IDomainService<TDomain, TIdentifier> domainService,
            IEnumerable<TIdentifier> ids, List<SortItem> sortItems)
                where TDomain : DomainEntityBase<TIdentifier>
        {
            return domainService.FindAll(e => ids.Contains(e.Id), sortItems);
        }

        public static IList<TDomain> FindAll<TDomain, TIdentifier>(
            this IDomainService<TDomain, TIdentifier> domainService,
            Criteria criteria, List<SortItem> sortItems)
            where TDomain : DomainEntityBase<TIdentifier>
        {
            return domainService.FindAll(ExpressionHelper.CreateFromCriteria<TDomain>(criteria), sortItems);
        }

        public static IList<TDomain> FindAll<TDomain, TIdentifier>(
            this IDomainService<TDomain, TIdentifier> domainService,
            Criteria criteria)
            where TDomain : DomainEntityBase<TIdentifier>
        {
            return domainService.FindAll(criteria, domainService.DefaultSortItems);
        }

        #endregion

        #region Find Page

        public static IPagedList<TDomain> FindPage<TDomain, TIdentifier>(
            this IDomainService<TDomain, TIdentifier> domainService,
            int page, int pageSize)
            where TDomain : DomainEntityBase<TIdentifier>
        {
            return domainService.FindPage(null, domainService.DefaultSortItems, page, pageSize);
        }

        public static IPagedList<TDomain> FindPage<TDomain, TIdentifier>(
            this IDomainService<TDomain, TIdentifier> domainService,
            List<SortItem> sortItems, int page, int pageSize)
            where TDomain : DomainEntityBase<TIdentifier>
        {
            return domainService.FindPage(null, domainService.DefaultSortItems, page, pageSize);
        }

        public static IPagedList<TDomain> FindPage<TDomain, TIdentifier>(
            this IDomainService<TDomain, TIdentifier> domainService,
            Expression<Func<TDomain, bool>> criteria, int page, int pageSize)
            where TDomain : DomainEntityBase<TIdentifier>
        {
            return domainService.FindPage(criteria, domainService.DefaultSortItems, page, pageSize);
        }


        public static IPagedList<TDomain> FindPage<TDomain, TIdentifier>(
            this IDomainService<TDomain, TIdentifier> domainService,
            Criteria criteria, List<SortItem> sortItems, int page, int pageSize)
            where TDomain : DomainEntityBase<TIdentifier>
        {
            return domainService.FindPage(ExpressionHelper.CreateFromCriteria<TDomain>(criteria), sortItems, page, pageSize);
        }

        public static IPagedList<TDomain> FindPage<TDomain, TIdentifier>(
            this IDomainService<TDomain, TIdentifier> domainService,
            Criteria criteria, int page, int pageSize)
            where TDomain : DomainEntityBase<TIdentifier>
        {
            return domainService.FindPage(criteria, domainService.DefaultSortItems, page, pageSize);
        }

        #endregion

        #region Find
        public static TDomain Find<TDomain, TIdentifier>(
            this IDomainService<TDomain, TIdentifier> domainService, TIdentifier id)
            where TDomain : DomainEntityBase<TIdentifier>
        {
            return domainService.Find(E => E.Id.Equals(id));
        }
        public static TDomain Find<TDomain, TIdentifier>(
            this IDomainService<TDomain, TIdentifier> domainService,
            TDomain entity)
            where TDomain : DomainEntityBase<TIdentifier>
        {
            return domainService.Find(entity.Id);
        }

        public static TDomain Find<TDomain, TIdentifier>(
            this IDomainService<TDomain, TIdentifier> domainService,
            Criteria criteria)
            where TDomain : DomainEntityBase<TIdentifier>
        {
            return domainService.Find(ExpressionHelper.CreateFromCriteria<TDomain>(criteria));
        }

        #endregion

        #region Count
        public static int Count<TDomain, TIdentifier>(
            this IDomainService<TDomain, TIdentifier> domainService)
            where TDomain : DomainEntityBase<TIdentifier>
        {
            return domainService.Count(D => true);
        }

        public static int Count<TDomain, TIdentifier>(
            this IDomainService<TDomain, TIdentifier> domainService, Criteria criteria)
            where TDomain : DomainEntityBase<TIdentifier>
        {
            return domainService.Count(ExpressionHelper.CreateFromCriteria<TDomain>(criteria));
        }
        #endregion
    }
}
