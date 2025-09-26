using LabXand.Core.ExceptionManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.DistributedServices.ClientProxy
{
    public class ClientProxyExceptionHandler : IExceptionHandler
    {
        public void Handle(Exception exception)
        {
            if (exception is FaultException)
            {
                throw new Exception(((FaultException)exception).Message);
            }
            throw new Exception("خطا در برقراری ارتباط با سرویس دهنده", exception);
        }
    }
}
