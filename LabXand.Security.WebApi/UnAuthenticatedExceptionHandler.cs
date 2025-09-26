using LabXand.Security.Core;
using System;
using System.Net;

namespace LabXand.Core.ExceptionManagement
{
    public class UnAuthenticatedExceptionHandler : CustomExceptionHandlerBase<UnAuthenticatedException>
    {
        public override HttpStatusCode HttpCode
        {
            get
            {
                return HttpStatusCode.Unauthorized;
            }
        }
        public override string GetUserMessage(Exception exception)
        {
            return exception.Message;
        }
    }
}
