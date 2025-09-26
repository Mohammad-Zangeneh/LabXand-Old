using System;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Net;

namespace LabXand.Core.ExceptionManagement
{
    public class EFExceptionHandler : CustomExceptionHandlerBase<EntityException>
    {
        public override string GetUserMessage(Exception exception)
        {
            return " مشکلی در ارتباط با پایگاه داده سیستم وجود دارد ";
        }

        public override HttpStatusCode HttpCode
        {
            get
            {
                return HttpStatusCode.ServiceUnavailable;
            }
        }
    }

    public class SqlExceptionHandler : CustomExceptionHandlerBase<SqlException>
    {
        public override string GetUserMessage(Exception exception)
        {
            return " مشکلی در ارتباط با پایگاه داده سیستم وجود دارد ";
        }

        public override HttpStatusCode HttpCode
        {
            get
            {
                return HttpStatusCode.ServiceUnavailable;
            }
        }
    }
}
