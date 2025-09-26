using LabXand.Core.ExceptionManagement;
using LabXand.Security.Core;
using LabXand.Security.Wcf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.DistributedServices.ClientProxy
{
    public abstract class SecureClientProxyBase<TService, TUserContext> : ClientProxyBase<TService>
        where TUserContext : class,IUserContext
    {

        public SecureClientProxyBase(IExceptionHandler exceptionHandler, IUserContextDetector<TUserContext> userContextDetector, ISecurityInitializer<TUserContext> securityInitializer)
            : base(exceptionHandler)
        {
            SecurityInitializer = securityInitializer;
            UserContextDetector = userContextDetector;
        }

        protected override void Initialize(ChannelFactory<TService> channel)
        {
            SecurityInitializer.Initialize(channel, UserContext);
        }
        protected TUserContext UserContext
        {
            get
            {
                if (UserContextDetector != null)
                    return UserContextDetector.UserContext;
                return null;
            }
        }
        protected ISecurityInitializer<TUserContext> SecurityInitializer { get; private set; }
        protected IUserContextDetector<TUserContext> UserContextDetector { get; private set; }
    }
}
