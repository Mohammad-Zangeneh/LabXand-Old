using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Security.Core
{
    public interface IUserContextDetector<TUserContext>
        where TUserContext : class,IUserContext
    {
        TUserContext UserContext { get; }
    }
}
