using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabXand.DomainLayer.Core
{
    public interface IDomainService<TDomain, TIdentifier> : IReadOnlyDomainService<TDomain, TIdentifier>
        where TDomain : DomainEntityBase<TIdentifier>
    {
        List<IServiceValidator<TDomain, TIdentifier>> OnSaveValidators { get; }
        List<IServiceValidator<TDomain, TIdentifier>> OnRemoveValidators { get; }
        TDomain Save(TDomain domain);
        bool Remove(TDomain domain);
    }
}
