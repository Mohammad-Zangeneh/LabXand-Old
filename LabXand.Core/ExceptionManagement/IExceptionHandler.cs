using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace LabXand.Core.ExceptionManagement
{
    public interface IExceptionHandler
    {
        bool CanHandle(Exception exception);
        string GetUserMessage(Exception exception);
        string GetTechnicalDetails(Exception exception);
        HttpStatusCode HttpCode { get; }
    }    

}
