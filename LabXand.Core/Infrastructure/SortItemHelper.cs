using LabXand.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;

namespace LabXand.Core
{
    public static class SortItemHelper
    {
        public static IQueryable<T> GetQueryable<T>(List<SortItem> sortItems, IQueryable<T> source) where T : class
        {
            Guard.ArgumentNotNull(source, "source");
            if (sortItems == null || sortItems.Count == 0)
                return source;

            string orderByClaus = sortItems.Aggregate("", (result, sortItem) => result + (string.IsNullOrWhiteSpace(result) ? "" : ",") + (sortItem.Direction == SortDirection.Descending ?
                     string.Format("{0} DESC", sortItem.SortFiledsSelector) : sortItem.SortFiledsSelector));
            return source.OrderBy(orderByClaus);
        }
    }
}
