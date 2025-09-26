using System;
using System.Net;

namespace LabXand.Core.ExceptionManagement
{
    public abstract class CustomExceptionHandlerBase<TException> : IExceptionHandler
        where TException : Exception
    {
        public virtual HttpStatusCode HttpCode
        {
            get
            {
                return HttpStatusCode.InternalServerError;
            }
        }

        public virtual string GetTechnicalDetails(Exception exception)
        {
            return ExceptionHelper.GetDetails(exception);
        }

        public abstract string GetUserMessage(Exception exception);

        public bool CanHandle(Exception exception)
        {
            return exception as TException != null;
        }
    }

}
