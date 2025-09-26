using LabXand.Logging.Core;
using System;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace LabXand.DistributedServices.WebApi
{
    public class ApiControllerBase : ApiController
    {
        protected T Invoke<T>(Func<T> method)
        {
            return method.Invoke();
        }

        protected LogEntryBase LogEntry;
        protected LogEntryBase CreateLogEntry(ServiceOperationTypes operationType)
        {
            LogEntryBase logEntry = new LogEntryBase(operationType);
            logEntry.Type = operationType;
            logEntry.Initiate();
            logEntry.SetMethodParameters(6);
            return logEntry;
        }

        protected virtual void BeforeInvoke(ServiceOperationTypes operationType)
        {
            LogEntry = CreateLogEntry(operationType);
            LogEntry.SetUserInformation(SetUserInformation());
        }

        private string GetClientIp(HttpRequestMessage request = null)
        {
            //request = request ?? Request;

            //if (request.Properties.ContainsKey("MS_HttpContext"))
            //{
            //    return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            //}
            //else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            //{
            //    RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
            //    return prop.Address;
            //}
            //else if (HttpContext.Current != null)
            //{
            //    return HttpContext.Current.Request.UserHostAddress;
            //}
            //else
            //{
            //    return null;
            //}
        }

        protected virtual UserInformationForLog SetUserInformation()
        {
            UserInformationForLog userInfoLog = new UserInformationForLog();
            userInfoLog.CallerIp = HttpContext.Current.Request.UserHostAddress;
            userInfoLog.CallerAgent = HttpContext.Current.Request.UserAgent;
            userInfoLog.CalledUrl = HttpContext.Current.Request.Url.OriginalString;
            return userInfoLog;
        }
    }
}