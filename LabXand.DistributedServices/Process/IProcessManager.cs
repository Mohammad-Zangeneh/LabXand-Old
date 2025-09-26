using LabXand.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.DistributedServices.Process
{
    public interface IProcessManager
    {
        ProcessActionResult StartProcess(IStartProcessRequest startProcessRequest);
        ProcessActionResult CompleteTask(ICompleteTaskRequest taskInformation);
    }

    public interface IProcessManager<TStartProcessRequest, TCompleteTaskRequest> : IProcessManager
        where TStartProcessRequest : IStartProcessRequest
        where TCompleteTaskRequest : ICompleteTaskRequest        
    {
        ProcessActionResult StartProcess(TStartProcessRequest startProcessRequest);
        ProcessActionResult CompleteTask(TCompleteTaskRequest taskInformation);
    }
}

