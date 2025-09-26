using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Security.Core
{
    public interface IUserContext
    {
        string UserName { get; set; }
        bool IsAuthorizedFor(string claim);
    }
}
