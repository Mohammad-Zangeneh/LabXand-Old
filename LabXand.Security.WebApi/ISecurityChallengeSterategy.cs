using LabXand.Security.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Security.WebApi
{
    public interface ISecurityChallengeSterategy<TUserContext> 
        where TUserContext : IUserContext
    {
        TUserContext CurrentUser { get; }
        bool IsAuthenticated { get; }
    }
}
