using System;
using System.Net;
using System.Text;
using System.Web;

namespace LabXand.Core.ExceptionManagement
{
    public class UnauthorizedAccessExceptionHandler : CustomExceptionHandlerBase<UnauthorizedAccessException>
    {
        public override string GetUserMessage(Exception exception)
        {
            return " خطای دسترسی غیرمجاز ";
        }

        public override string GetTechnicalDetails(Exception exception)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string message = exception.Message;
            string[] splitedMessage = message.Split('$');
            if (splitedMessage.Length > 1)
            {
                string userName = splitedMessage[0];
                string operationCode = splitedMessage[1];
                stringBuilder.AppendLine(string.Format("User '{0}' don't have '{1}' permission.", userName, operationCode));
            }
            else
                stringBuilder.AppendLine(string.Format("User don't have {0} permission.", exception.Message));

            stringBuilder.AppendLine(base.GetTechnicalDetails(exception));
            return stringBuilder.ToString();
        }
        public override HttpStatusCode HttpCode
        {
            get
            {
                return HttpStatusCode.Forbidden;
            }
        }
    }

}
