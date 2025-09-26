using LabXand.Infrastructure.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Infrastructure.Data.EF.UpdateConfiguration
{
    public interface IPropertyUpdater<TRoot>
        where TRoot : class
    {
        void Update(DbContextBase context, TRoot currentValue, TRoot originalValue);       
        IPropertyUpdaterCustomizer<TRoot> PropertyUpdaterCustomizer { get; }
    }
}
