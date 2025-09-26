using LabXand.Extensions;
using LabXand.DomainLayer.Core;
using LabXand.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.DomainLayer
{
    public class SingleValueFieldMustBeUnique<TDomain, TIdentifier> : UniquenessValidationBase<TDomain, TIdentifier>
            where TDomain : DomainEntityBase<TIdentifier>
    {
        public SingleValueFieldMustBeUnique(IReadOnlyDomainService<TDomain, TIdentifier> service, Func<Expression<Func<TDomain, bool>>, int> countMethod)
            : base(service, "Id", countMethod)
        {
        }

        public SingleValueFieldMustBeUnique(IReadOnlyDomainService<TDomain, TIdentifier> service, string idPropertyName)
            : base(service, idPropertyName)
        {
        }

        public SingleValueFieldMustBeUnique(IReadOnlyDomainService<TDomain, TIdentifier> service, string idPropertyName, Func<Expression<Func<TDomain, bool>>, int> countMethod)
            : base(service, idPropertyName, countMethod)
        {
        }

        public List<List<string>> FieldSpecifications { get; set; }

        public override List<string> BrokenRules(TDomain domain)
        {
            if (FieldSpecifications != null)
            {
                Type domainType = typeof(TDomain);
                ParameterExpression parameter = Expression.Parameter(typeof(TDomain));

                foreach (var items in FieldSpecifications)
                {
                    BinaryExpression binaryExpression = null;
                    if (domain.Id != null && !domain.Id.Equals(default(TIdentifier)))
                        binaryExpression = ExpressionHelper.CreateNotEqulityExpression<TDomain>(parameter, _idPropertyName, ExpressionHelper.GetPropertyValue(domain, _idPropertyName));
                    //TypeHelper.GetPropertyValue(domain, _idPropertyName));
                    bool hasExpression = false;
                    foreach (var field in items)
                    {
                        //object propertyValue = TypeHelper.GetPropertyValue(domain, field);
                        object propertyValue = ExpressionHelper.GetPropertyValue(domain, field);
                        //Expression.Lambda<Func<TDomain, object>>(ExpressionHelper.GetMemberExpression<TDomain>(parameter, field), parameter).Compile().Invoke(domain);
                        if (propertyValue != null && !string.IsNullOrWhiteSpace(propertyValue.ToString()))
                        {
                            hasExpression = true;
                            var expression = ExpressionHelper.CreateEqulityExpression<TDomain>(parameter, field, propertyValue);
                            if (binaryExpression == null)
                                binaryExpression = expression;
                            else
                                binaryExpression = Expression.And(binaryExpression, expression);
                        }
                    }
                    if (hasExpression)
                    {
                        int recordCount = _CountMethod.Invoke(ExpressionHelper.Rewrite<TDomain>(
                            Expression.Lambda<Func<TDomain, bool>>(binaryExpression, parameter), parameter));
                        if (recordCount > 0)
                        {
                            string fields = string.Empty;
                            items.ForEach(i => fields += string.Format("{0}, ", TypeHelper.GetPropertyDisplayName<TDomain>(i)));
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
