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
    public class UpdateCollectionConfiguration<TRoot, T, I> : UpdateConfigarationBase<TRoot>
        where TRoot : class
        where T : class, IDomainEntity<I>
    {
        public UpdateCollectionConfiguration(IPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer, List<string> constantFields)
            : base(propertyUpdaterCustomizer, constantFields)
        {
            InnerConfigurations = new List<object>();
        }

        public UpdateCollectionConfiguration(List<string> constantFields)
            : base(new EmptyPropertyUpdaterCustomizer<TRoot>(), constantFields)
        {
            InnerConfigurations = new List<object>();
        }

        public UpdateCollectionConfiguration()
            : this(new List<string>())
        {
        }

        public Expression<Func<TRoot, ICollection<T>>> ItemSelector { get; set; }
        public string ParentPropertyName { get; set; }
        public string ChildtPropertyName { get; set; }
        public override IPropertyUpdater<TRoot> CreateUpdater(IPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer)
        {
            return new CollectionNavigationPropertyUpdater<TRoot, T, I>(propertyUpdaterCustomizer, this);
        }
    }
}
