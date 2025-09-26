using LabXand.Security.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Security.Wcf
{
    public interface ISecurityInitializer<TUserContext>
        where TUserContext : class,IUserContext
    {
        void Initialize(ChannelFactory channel, TUserContext userContext);
    }
}
