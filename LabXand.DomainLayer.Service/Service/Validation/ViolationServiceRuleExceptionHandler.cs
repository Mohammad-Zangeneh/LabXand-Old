using LabXand.Core.ExceptionManagement;
using System;
using System.Net;

namespace LabXand.DomainLayer.Service.Validation
{
    public class ViolationServiceRuleExceptionHandler : CustomExceptionHandlerBase<ViolationServiceRuleException>
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
