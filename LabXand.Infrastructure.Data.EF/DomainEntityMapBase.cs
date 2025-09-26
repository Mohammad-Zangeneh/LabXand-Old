using LabXand.DomainLayer.Core;
using System.Data.Entity.ModelConfiguration;

namespace LabXand.Infrastructure.Data.EF
{
    public abstract class DomainEntityMapBase<TDomain, TIdentifier> : EntityTypeConfiguration<TDomain>
        where TDomain : DomainEntityBase<TIdentifier>
    {
        protected DomainEntityMapBase()
        {
            ConfigurePrimaryKey();
        }

        protected virtual void ConfigurePrimaryKey()
        {
            HasKey(t => t.Id);
        }
    }
}
