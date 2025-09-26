using LabXand.Core.ExceptionManagement;
using LabXand.Logging.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LabXand.DistributedServices.WebApi
{
    public static class WebExceptionHandler
    {
        public static void PrepareResponseWhenExceptionOccurred()
        {
            HttpContext httpContext = HttpContext.Current;
            if (httpContext != null)
            {
                RequestContext requestContext = ((MvcHandler)httpContext.CurrentHandler).RequestContext;
                Exception exception = HttpServerUtility.GetLastError(); ;
                Core.ExceptionManagement.IExceptionHandler handler = ExceptionHandlerFactory.GetSuitableHandler(context.Exception);
                new FileLogger().Log(handler.GetTechnicalDetails(context.Exception));
                context.Result = new TextPlainErrorResult
                {
                    Request = context.ExceptionContext.Request,
                    Content = handler.GetUserMessage(context.Exception),
                    HttpCode = handler.HttpCode
                };
                /* When the request is ajax the system can automatically handle a mistake with a JSON response. 
                   Then overwrites the default response */
                if (requestContext.HttpContext.Request.IsAjaxRequest())
                {
                    httpContext.Response.Clear();
                    string controllerName = requestContext.RouteData.GetRequiredString("controller");
                    IControllerFactory factory = ControllerBuilder.Current.GetControllerFactory();
                    IController controller = factory.CreateController(requestContext, controllerName);
                    ControllerContext controllerContext = new ControllerContext(requestContext, (ControllerBase)controller);

                    JsonResult jsonResult = new JsonResult
                    {
                        Data = new { success = false, serverError = "500" },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                    jsonResult.ExecuteResult(controllerContext);
                    httpContext.Response.End();
                }
                else
                {
                    httpContext.Response.Redirect("~/Error");
                }
            }
        }
    }
}