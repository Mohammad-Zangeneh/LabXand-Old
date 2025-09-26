using LabXand.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Linq.Expressions;
using LabXand.Extensions;
using System.Diagnostics;

namespace LabXand.Infrastructure.Data.EF
{

    public static class QueryableExtensions
    {
        public static IQueryable<T> Include<T>(this IQueryable<T> query, List<string> includeNavigationProperties)
            where T : class
        {
            if (includeNavigationProperties != null && includeNavigationProperties.Count > 0)
                //return includeNavigationProperties.Aggregate(query, (current, item) => current.Include(item));
                foreach (var item in includeNavigationProperties)
                {
                        query = query.Include(item);                 
                }
            return query;
        }

        public static IQueryable<T> Include<T>(this IQueryable<T> query, List<Expression<Func<T, dynamic>>> navigaionProperties)
            where T : class
        {
            if (navigaionProperties != null)
            {
                List<string> result = new List<string>();
                navigaionProperties.ForEach(n => result.Add(ExpressionHelper.GetNameOfProperty(n)));
                return query.Include(result);
            }
            return query;
        }

        public static IQueryable<T> ApplyPredicate<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate)
            where T : class
        {
            if (predicate != null)
                query = query.Where(predicate);

            return query;
        }
    }

}
