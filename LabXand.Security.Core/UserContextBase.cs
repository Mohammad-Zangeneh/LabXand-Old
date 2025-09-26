using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Security.Core
{
    public abstract class UserContextBase : IUserContext
    {
        public UserContextBase()
        {

        }

        public string UserName { get; set; }        

        public abstract bool IsAuthorizedFor(string claim);
    }
}
