using LabXand.Infrastructure.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Infrastructure.Data.EF.UpdateConfiguration
{
    public abstract class PropertyUpdaterBase<TRoot> : IPropertyUpdater<TRoot>
        where TRoot : class
    {
        protected PropertyUpdaterBase(IPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer)
        {
            _propertyUpdaterCustomizer = propertyUpdaterCustomizer;
        }
        public abstract void Update(DbContextBase context, TRoot currentValue, TRoot originalValue);
        IPropertyUpdaterCustomizer<TRoot> _propertyUpdaterCustomizer;
        public IPropertyUpdaterCustomizer<TRoot> PropertyUpdaterCustomizer
        {
            get
            { return _propertyUpdaterCustomizer; }
        }
    }
}
