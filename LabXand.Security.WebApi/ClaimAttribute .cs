using LabXand.Security.Core;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace LabXand.Security.WebApi
{
    public class ClaimAttribute : AuthorizeAttribute
    {

        public string Code { get; set; }
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (HttpContext.Current.User == null)
            {
                throw new UnAuthenticatedException();
            }
            CustomeAuthetication(actionContext);
            IUserContext userContext = HttpContext.Current.User as IUserContext;
            if (userContext == null)
            {
                throw new UnAuthenticatedException();
            }
            if (!string.IsNullOrWhiteSpace(Code) && !userContext.IsAuthorizedFor(Code))
            {
                throw new UnauthorizedAccessException(string.Format("{0}${1}", userContext.UserName, Code));
            }
            return;
        }

        protected virtual void CustomeAuthetication(HttpActionContext actionContext)
        {

        }
    }
}
