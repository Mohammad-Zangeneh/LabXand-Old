using LabXand.Security.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.DomainLayer.Core
{
    public interface ISecurityEnabledService<TUserContext>
        where TUserContext : IUserContext
    {
        TUserContext UserContext { get; }
    }
}
