
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Infrastructure.Data.EF.UpdateConfiguration
{
    public interface IUpdateConfiguration<TRoot>
        where TRoot : class
    {
        IEnumerable InnerConfigurations { get; set; }
        IPropertyUpdaterCustomizer<TRoot> PropertyUpdaterCustomizer { get; }
        IPropertyUpdater<TRoot> CreateUpdater(IPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer);
        void AddInnerConfigurations(IUpdateConfiguration<object> innerConfiguration);
        List<string> ConstantFields { get; set; }
    }
}
