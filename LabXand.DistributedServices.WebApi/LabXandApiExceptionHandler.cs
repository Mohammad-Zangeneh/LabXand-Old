using LabXand.Core.ExceptionManagement;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using LabXand.Logging.Core;
using System;
using System.Web.Http;
using System.Text;

namespace LabXand.DistributedServices.WebApi
{
    public class LabXandApiExceptionHandler : System.Web.Http.ExceptionHandling.IExceptionHandler
    {
        private readonly ILogContext<ApiLogEntry> _logContext;

        public LabXandApiExceptionHandler(ILogContext<ApiLogEntry> logContext)
        {
            _logContext = logContext;
        }

        public virtual Task HandleAsync(ExceptionHandlerContext context,
                                    CancellationToken cancellationToken)
        {
            return HandleAsyncCore(context, cancellationToken);
        }

        public async virtual Task HandleAsyncCore(ExceptionHandlerContext context,
                                           CancellationToken cancellationToken)
        {
            await HandleCore(context);
            return;
        }

        public async virtual Task HandleCore(ExceptionHandlerContext context)
        {
            try
            {
                Core.ExceptionManagement.IExceptionHandler handler = ExceptionHandlerFactory.GetSuitableHandler(context.Exception);
                string errorDetails = handler.GetTechnicalDetails(context.Exception);
                string message = handler.GetUserMessage(context.Exception);
                _logContext.Current.ExceptionOccured(message, ExceptionInformation.CreateFromException(_logContext.Current.Id, context.Exception));
                context.Result = new TextPlainErrorResult
                {
                    Request = context.ExceptionContext.Request,
                    Content = message,
                    HttpCode = handler.HttpCode
                };
                new FileLogger().Log(errorDetails, new ExceptionLogPathCreator(context.Exception));
            }
            catch (Exception ex)
            {
                string message = await GetDetails(context.Exception);
                message += Environment.NewLine + GetDetails(ex);
                new FileLogger().Log(message, new ExceptionLogPathCreator(context.Exception));
            }
        }

        public async Task<string> GetDetails(Exception exception)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(string.Format("#. error at {0}.", DateTime.Now));
            while (exception != null)
            {
                stringBuilder.AppendLine(exception.GetType().ToString());
                stringBuilder.AppendLine(exception.Message);
                var webException = exception as HttpResponseException;
                if (webException != null)
                {
                    stringBuilder.AppendLine("<<");
                    stringBuilder.AppendLine(await webException.Response.Content.ReadAsStringAsync());
                    stringBuilder.AppendLine(">>");
                }

                stringBuilder.AppendLine(exception.StackTrace);
                exception = exception.InnerException;
            }
            return stringBuilder.ToString();
        }
    }
}