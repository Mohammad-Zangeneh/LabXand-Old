using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabXand.Core
{
    public interface IEntityComparer<T>
    {
        List<Tuple<Func<T, dynamic>, Func<object, object, bool>>> PropertySpecifications { get; set; }
        int Compare(T current, T other);
    }
}
