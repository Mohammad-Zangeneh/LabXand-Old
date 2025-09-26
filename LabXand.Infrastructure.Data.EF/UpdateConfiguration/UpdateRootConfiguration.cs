
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Infrastructure.Data.EF.UpdateConfiguration
{
    public class UpdateRootConfiguration<TRoot> : UpdateConfigarationBase<TRoot>
        where TRoot : class
    {
        public UpdateRootConfiguration(IPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer, List<string> constantFields)
            : base(propertyUpdaterCustomizer, constantFields)
        {
            InnerConfigurations = new List<object>();
        }
        public UpdateRootConfiguration(List<string> constantFields)
            : this(new EmptyPropertyUpdaterCustomizer<TRoot>(), constantFields)
        {

        }

        public override IPropertyUpdater<TRoot> CreateUpdater(IPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer)
        {
            return new RootPropertyUpdater<TRoot>(propertyUpdaterCustomizer, this);
        }
    }
}
