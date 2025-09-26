using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabXand.Infrastructure.Data.EF.UpdateConfiguration;
using LabXand.Infrastructure.Data.EF;

namespace LabXand.Infrastructure.Data.EF.UpdateConfiguration
{
    internal class RootPropertyUpdater<TRoot> : PropertyUpdaterBase<TRoot>
        where TRoot : class
    {
        public RootPropertyUpdater(IPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer, IUpdateConfiguration<TRoot> configuration)
            : base(propertyUpdaterCustomizer)
        {
            updateConfiguration = configuration;
        }

        public RootPropertyUpdater(IUpdateConfiguration<TRoot> configuration)
            : this(new EmptyPropertyUpdaterCustomizer<TRoot>(), configuration)
        {

        }

        IUpdateConfiguration<TRoot> updateConfiguration;
        public override void Update(DbContextBase context, TRoot currentValue, TRoot originalValue)
        {
            context.SafeUpdate<TRoot>(currentValue, originalValue, updateConfiguration.ConstantFields);
            if (updateConfiguration.InnerConfigurations != null)
            {
                foreach (var updateConfig in updateConfiguration.InnerConfigurations)
                {
                    IUpdateConfiguration<TRoot> tempconfig = updateConfig as IUpdateConfiguration<TRoot>;
                    if (tempconfig != null)
                        tempconfig.CreateUpdater().Update(context, currentValue, originalValue);
                }
            }
        }
    }
}
