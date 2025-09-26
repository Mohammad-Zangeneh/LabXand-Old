using LabXand.DomainLayer.Core;
using LabXand.Security.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.DomainLayer
{
    public abstract class SecurityEnabledDomainServiceBase<TDomain, TRepository, TIdentifier, TUserContext> : DomainServiceBase<TDomain, TRepository, TIdentifier>, ISecurityEnabledDomainService<TDomain, TIdentifier, TUserContext>
        where TRepository : class,IRepository<TDomain, TIdentifier>
        where TDomain : DomainEntityBase<TIdentifier>
        where TUserContext : class,IUserContext
    {
        public SecurityEnabledDomainServiceBase(TRepository repository, IUserContextDetector<TUserContext> userContextDetector)
            : base(repository)
        {
            _userContextDetector = userContextDetector;
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
        public override TDomain Save(TDomain domain)
        {
            if (domain is ITracableEntity<TUserContext>)
            {
                ((ITracableEntity<TUserContext>)domain).SetTraceData(UserContext);
            }

            return base.Save(domain);
        }
    }
}
