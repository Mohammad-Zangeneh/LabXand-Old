
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
    public class UpdateManyToManyCollection<TRoot, T, I> : UpdateConfigarationBase<TRoot>
        where TRoot : class
        where T : class, IDomainEntity<I>
    {
        public UpdateManyToManyCollection(IPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer, List<string> constantFields)
            : base(propertyUpdaterCustomizer, constantFields)
        {
            InnerConfigurations = new List<object>();
        }
        public UpdateManyToManyCollection()
            : this(new EmptyPropertyUpdaterCustomizer<TRoot>(), new List<string>())
        {
        }
        public Expression<Func<TRoot, ICollection<T>>> ItemSelector { get; set; }

        public override IPropertyUpdater<TRoot> CreateUpdater(IPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer)
        {
            return new ManyToManyUpdater<TRoot, T, I>(propertyUpdaterCustomizer, this);
        }
    }
}
