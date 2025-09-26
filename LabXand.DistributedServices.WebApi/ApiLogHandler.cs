using LabXand.Core.ExceptionManagement;
using LabXand.Logging.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace LabXand.DistributedServices.WebApi
{
    public class ApiLogHandler : DelegatingHandler
    {
        private readonly ILogger _logger;
        private readonly ILogContext<ApiLogEntry> _logContext;

        public ApiLogHandler(ILogger logger, ILogContext<ApiLogEntry> logContext)
        {
            _logger = logger;
            _logContext = logContext;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var enableLogConfig = System.Configuration.ConfigurationManager.AppSettings["EnabledLog"];
            var enableLog = enableLogConfig.ToLower() == "true";
            var apiLogEntry = CreateApiLogEntryWithRequestData(request);
            if (request.Content != null)
            {
                await request.Content.ReadAsStringAsync()
                    .ContinueWith(task =>
                    {
                        apiLogEntry.RequestContentBody = string.Empty;
                        apiLogEntry.RequestContentBody = GetRequestContent(task.Result);
                        apiLogEntry.RequestContentLength = task.Result.Length;
                    }, cancellationToken);
            }
            _logContext.InitiateEntry(apiLogEntry);
            HttpResponseMessage response = null;
            try
            {
                response = await base.SendAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                var exceptionHandler = ExceptionHandlerFactory.GetSuitableHandler(ex);
                string message = exceptionHandler.GetUserMessage(ex);
                apiLogEntry.ExceptionOccured(message, ExceptionInformation.CreateFromException(_logContext.Current.Id, ex));
                response = new HttpResponseMessage(exceptionHandler.HttpCode)
                {
                    Content = new StringContent(message)
                };
            }
            finally
            {
                apiLogEntry.ResponseStatusCode = (int)response.StatusCode;
                apiLogEntry.ResponseTimestamp = DateTime.Now;
                if (response.Content != null)
                {
                    apiLogEntry.ResponseContentLength = response.Content.ReadAsStringAsync().Result.Length;
                    apiLogEntry.ResponseContentType = response.Content.Headers.ContentType.MediaType;
                    apiLogEntry.ResponseHeaders = SerializeHeaders(response.Content.Headers);
                }
                CustomizeLogEntryInit(ref apiLogEntry);
                if (enableLog)
                    _logger.Log(apiLogEntry);
            }
            return response;
        }
        private string removeFileData(string content)
        {
            string ff = "\"fileData\":\"";
            var f = content.IndexOf(ff);
            if (f != -1)
            {
                var end = content.IndexOf("\",\"mIME\"", f);
                int lenght = (end - f + 2);
                if (lenght >= 0)
                {
                    var tt = content.Substring(f, lenght);
                    if (!string.IsNullOrWhiteSpace(tt))
                        content = content.Replace(tt, "");
                    if (content.IndexOf(ff) != -1)
                        return removeFileData(content);
                }
            }
            return content;
        }
        private string GetRequestContent(string content)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                string ff = "\"fileData\":\"";
                var f = content.IndexOf(ff);
                if (f != -1)
                {
                    content = removeFileData(content);
                }
            }
            return content;
        }

        private ApiLogEntry CreateApiLogEntryWithRequestData(HttpRequestMessage request)
        {
            var context = ((HttpContextBase)request.Properties["MS_HttpContext"]);
            var routeData = request.GetRouteData();
            var config = GlobalConfiguration.Configuration;
            var controllerSelector = new DefaultHttpControllerSelector(config);

            // descriptor here will contain information about the controller to which the request will be routed. If it's null (i.e. controller not found), it will throw an exception
            var descriptor = controllerSelector.SelectController(request);
            var controllerContext = new HttpControllerContext(config, routeData, request)
            {
                ControllerDescriptor = descriptor
            };
            var actionMapping = new ApiControllerActionSelector().SelectAction(controllerContext);
            var logEntry = new ApiLogEntry
            {
                Application = System.Configuration.ConfigurationManager.AppSettings["ApplicationName"],
                ControllerName = descriptor.ControllerName,
                ActionName = actionMapping.ActionName,
                User = context.User.Identity.Name,
                Machine = Environment.MachineName,
                RequestContentType = context.Request.ContentType,
                RequestRouteTemplate = routeData.Route.RouteTemplate,
                RequestIpAddress = context.Request.UserHostAddress,
                RequestMethod = request.Method.Method,
                RequestHeaders = SerializeHeaders(request.Headers),
                RequestTimestamp = DateTime.Now,
                RequestUri = request.RequestUri.ToString()
            };
            var methodInfo = controllerContext.ControllerDescriptor.ControllerType.GetMethod(actionMapping.ActionName);
            if (methodInfo != null)
            {
                var descriptionAttribute = (DescriptionAttribute)methodInfo.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault();
                if (descriptionAttribute != null)
                {
                    logEntry.Description = descriptionAttribute.Description;
                }

            }

            return logEntry;
        }

        protected virtual void CustomizeLogEntryInit(ref ApiLogEntry apiLogEntry)
        { }
        private string SerializeHeaders(HttpHeaders headers)
        {
            var dict = new Dictionary<string, string>();

            foreach (var item in headers.ToList())
            {
                if (item.Value != null)
                {
                    var header = string.Empty;
                    foreach (var value in item.Value)
                    {
                        header += value + " ";
                    }

                    // Trim the trailing space and add item to the dictionary
                    header = header.TrimEnd(" ".ToCharArray());
                    dict.Add(item.Key, header);
                }
            }

            return JsonConvert.SerializeObject(dict, Formatting.Indented);
        }
    }
}