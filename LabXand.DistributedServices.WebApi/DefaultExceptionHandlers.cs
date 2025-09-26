using LabXand.Core.ExceptionManagement;
using LabXand.DomainLayer.Service.Validation;
using System.Collections.Generic;

namespace LabXand.DistributedServices.WebApi
{
    public static class DefaultExceptionHandlers
    {
        public static List<IExceptionHandler> Get
        {
            get
            {
                List<IExceptionHandler> list = new List<IExceptionHandler>
                {
                    new DefaultExceptionHandler(),
                    new ViolationServiceRuleExceptionHandler(),
                    new UnAuthenticatedExceptionHandler(),
                    new UnauthorizedAccessExceptionHandler(),
                    new EFExceptionHandler(),
                    new SqlExceptionHandler(),
                    new InvalidDomainExceptionHandler(),
                    new DbUpdateExceptionHandler()
                };
                return list;
            }
        }
    }
}
