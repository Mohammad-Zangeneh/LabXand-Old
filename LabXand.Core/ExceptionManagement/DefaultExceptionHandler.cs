using System;
using System.Net;

namespace LabXand.Core.ExceptionManagement
{
    public class DefaultExceptionHandler : IExceptionHandler
    {
        public HttpStatusCode HttpCode
        {
            get
            {
                return HttpStatusCode.BadRequest;
            }
        }

        public bool CanHandle(Exception exception)
        {
            return exception as ExceptionBase != null;
        }

        public string GetTechnicalDetails(Exception exception)
        {
            return ExceptionHelper.GetDetails(exception);
        }

        public string GetUserMessage(Exception exception)
        {
            return exception.Message;
        }
    }
}
