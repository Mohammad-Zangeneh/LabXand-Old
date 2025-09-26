using System;
using System.Collections.Generic;
using System.Linq;

namespace LabXand.Core.ExceptionManagement
{
    public static class ExceptionHandlerFactory
    {
        static ExceptionHandlerFactory()
        {
            ExceptionHandlers = new List<IExceptionHandler>();
        }
        public static void RegisterExceptionHandler(List<IExceptionHandler> exceptionHandlers)
        {
            exceptionHandlers.ForEach(e => RegisterExceptionHandler(e));
        }
        public static void RegisterExceptionHandler(IExceptionHandler exceptionHandler)
        {
            ExceptionHandlers.Add(exceptionHandler);
        }
        private static List<IExceptionHandler> ExceptionHandlers { get; set; }
        public static IExceptionHandler GetSuitableHandler(Exception exception)
        {
            IExceptionHandler result = null;
            result = ExceptionHandlers.FirstOrDefault(e => e.CanHandle(exception));
            if (result == null)
                return new UnHandledExceptionHandler();
            return result;
        }
    }    

}
