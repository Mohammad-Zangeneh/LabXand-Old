using LabXand.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LabXand.Extensions;
using LabXand.DomainLayer.Core;
using LabXand.Security.Core;

namespace LabXand.DomainLayer
{
    public abstract class SecurityEnabledReadOnlyDomainServiceBase<TDomain, TRepository, TIdentifier, TUserContext> : ReadOnlyDomainServiceBase<TDomain, TRepository, TIdentifier>, ISecurityEnabledReadOnlyService<TDomain, TIdentifier, TUserContext>
        where TDomain : DomainEntityBase<TIdentifier>
        where TRepository : IRepository<TDomain, TIdentifier>
        where TUserContext : class, IUserContext
    {
        public SecurityEnabledReadOnlyDomainServiceBase(TRepository repository, IUserContextDetector<TUserContext> userContextInitializer)
            : base(repository)
        {
            _userContextDetector = userContextInitializer;
        }
        IUserContextDetector<TUserContext> _userContextDetector;
        public TUserContext UserContext
        {
            get
            {
                if (_userContextDetector != null)
                    return _userContextDetector.UserContext;
                return null;
            }
        }
    }
}
