using LabXand.Core.ExceptionManagement;
using LabXand.DomainLayer.Core;
using System;
using System.Net;

namespace LabXand.DomainLayer.Service.Validation
{
    public class InvalidDomainExceptionHandler : CustomExceptionHandlerBase<InvalidDomainException>
    {
        public override string GetUserMessage(Exception exception)
        {
            return exception.Message;
        }

        public override HttpStatusCode HttpCode
        {
            get
            {
                return HttpStatusCode.PreconditionFailed;
            }
        }
    }
}
