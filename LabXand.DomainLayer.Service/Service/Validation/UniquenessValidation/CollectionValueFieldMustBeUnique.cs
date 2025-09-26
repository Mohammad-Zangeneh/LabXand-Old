using LabXand.DomainLayer.Core;
using LabXand.Core;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LabXand.DomainLayer
{
    public class CollectionValueFieldMustBeUnique<TDomain, TProperty, TIdentifier> : UniquenessValidationBase<TDomain, TIdentifier>
            where TDomain : DomainEntityBase<TIdentifier>
    {
        public CollectionValueFieldMustBeUnique(IReadOnlyDomainService<TDomain, TIdentifier> service, Func<TDomain, IEnumerable<TProperty>> collectionPropertySelector)
            : this(service, "Id", collectionPropertySelector)
        {
        }

        public CollectionValueFieldMustBeUnique(IReadOnlyDomainService<TDomain, TIdentifier> service, string idPropertyName, Func<TDomain, IEnumerable<TProperty>> collectionPropertySelector)
            : base(service, idPropertyName)
        {
            CollectionPropertySelector = collectionPropertySelector;
        }

        public Func<TDomain, IEnumerable<TProperty>> CollectionPropertySelector { get; private set; }
        public List<string> FieldSpecifications { get; set; }

        public override List<string> BrokenRules(TDomain domain)
        {
            if (FieldSpecifications != null)
            {
                Type domainType = typeof(TDomain);
                ParameterExpression parameter = Expression.Parameter(domainType);
                IEnumerable<TProperty> propertyValues = CollectionPropertySelector(domain);

                if (propertyValues != null)
                {
                    bool hasExpression = false;
                    Criteria criteria = new EmptyCriteria();
                    if (domain.Id != null && !domain.Id.Equals(default(TIdentifier)))
                        criteria = criteria.NotEqual(_idPropertyName, TypeHelper.GetPropertyValue(domain, _idPropertyName), true);

                    foreach (TProperty item in propertyValues)
                    {
                        Criteria criteriaForValue = new EmptyCriteria();
                        foreach (var fieldSpecification in FieldSpecifications)
                        {
                            criteriaForValue = criteriaForValue.Equal(fieldSpecification, TypeHelper.GetPropertyValue(item, fieldSpecification.Substring(fieldSpecification.LastIndexOf(".") + 1)), true);
                        }
                        criteria = criteria.And(criteriaForValue);
                        int recordCount = _CountMethod.Invoke(Expression.Lambda<Func<TDomain, bool>>(criteria.GetExpression(parameter), parameter));
                        if (recordCount > 0)
                        {
                            string fields = string.Empty;
                            FieldSpecifications.ForEach(i => fields += string.Format("{0}, ", TypeHelper.GetPropertyDisplayName<TDomain>(i)));
                            fields = fields.Substring(0, fields.LastIndexOf(","));
                            fields = string.Format("{0} تکراری می باشد!!!!", fields);
                            return new List<string>() { fields };
                        }
                    }                    
                }
                return new List<string>();
            }
            else
                return new List<string>();
        }
    }
}
