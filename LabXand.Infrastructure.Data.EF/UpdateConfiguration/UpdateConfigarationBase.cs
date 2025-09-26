
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Infrastructure.Data.EF.UpdateConfiguration
{
    public abstract class UpdateConfigarationBase<TRoot> : IUpdateConfiguration<TRoot>
        where TRoot : class
    {
        protected UpdateConfigarationBase(IPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer,List<string> constantFields)
        {
            _propertyUpdaterCustomizer = propertyUpdaterCustomizer;
            ConstantFields = constantFields;
        }
        public IEnumerable InnerConfigurations { get; set; }
        IPropertyUpdaterCustomizer<TRoot> _propertyUpdaterCustomizer;
        public IPropertyUpdaterCustomizer<TRoot> PropertyUpdaterCustomizer { get { return _propertyUpdaterCustomizer; } }

        public List<string> ConstantFields { get; set; }
        
        public void AddInnerConfigurations(IUpdateConfiguration<object> innerConfiguration)
        {
            if (InnerConfigurations == null)
                InnerConfigurations = new List<object>();
            ((List<object>)InnerConfigurations).Add(innerConfiguration);
        }

        public abstract IPropertyUpdater<TRoot> CreateUpdater(IPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer);

    }
}
