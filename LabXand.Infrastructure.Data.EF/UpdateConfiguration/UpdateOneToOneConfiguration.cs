using LabXand.DomainLayer.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Infrastructure.Data.EF.UpdateConfiguration
{
    public class UpdateOneEntityConfiguration<TRoot, T, I> : UpdateConfigarationBase<TRoot>
        where TRoot : class
        where T : class, IDomainEntity<I>
    {
        public UpdateOneEntityConfiguration(IPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer, List<string> constantFields)
            : base(propertyUpdaterCustomizer, constantFields)
        {
            InnerConfigurations = new List<object>();
        }
        public UpdateOneEntityConfiguration()
            : this(new EmptyPropertyUpdaterCustomizer<TRoot>(), new List<string>())
        {
        }
        public Expression<Func<TRoot, T>> ItemSelector { get; set; }
        public string ParentPropertyName { get; set; }
        public string ChildtPropertyName { get; set; }
        public override IPropertyUpdater<TRoot> CreateUpdater(IPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer)
        {
            return new OneNavigationPropertyUpdater<TRoot, T, I>(propertyUpdaterCustomizer, this);
        }
    }
}
