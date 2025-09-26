using System;
using System.IO;
using System.Net;
using System.Text;

namespace LabXand.Core.ExceptionManagement
{
    public static class ExceptionHelper
    {
        public static string GetDetails(Exception exception)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(string.Format("#. error at {0}.", DateTime.Now));
            while (exception != null)
            {
                stringBuilder.AppendLine(exception.Message);
                stringBuilder.AppendLine(exception.GetType().ToString());
                var webException = exception as WebException;
                if (webException != null)
                {
                    if (webException.Response != null)
                        stringBuilder.AppendLine(new StreamReader(webException.Response.GetResponseStream()).ReadToEnd());
                    else
                        stringBuilder.AppendLine(webException.Message);
                }

                stringBuilder.AppendLine(exception.StackTrace);
                exception = exception.InnerException;
            }
            return stringBuilder.ToString();
        }
    }
}
