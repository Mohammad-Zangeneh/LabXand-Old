using System;
using System.Net;

namespace LabXand.Core.ExceptionManagement
{
    public class UnHandledExceptionHandler : IExceptionHandler
    {
        public HttpStatusCode HttpCode
        {
            get
            {
                return HttpStatusCode.ServiceUnavailable;
            }
        }

        public bool CanHandle(Exception exception)
        {
            return true;
        }

        public string GetTechnicalDetails(Exception exception)
        {
            return ExceptionHelper.GetDetails(exception);
        }

        public string GetUserMessage(Exception exception)
        {
            return "خطا در اجرای برنامه";
        }
    }
}
