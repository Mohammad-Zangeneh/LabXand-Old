using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Security.Core
{

    [Serializable]
    public class UnAuthorizedException : Exception
    {
        public UnAuthorizedException(string permissionCode) : base(string.Format("You don't have '{0}' permission.", permissionCode)) {
            PermissionCode = permissionCode;
        }
        public UnAuthorizedException(string permissionCode, Exception inner) : base(string.Format("You don't have '{0}' permission.", permissionCode), inner) {
            PermissionCode = permissionCode;
        }
        protected UnAuthorizedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        public string PermissionCode { get; set; }
    }
}
