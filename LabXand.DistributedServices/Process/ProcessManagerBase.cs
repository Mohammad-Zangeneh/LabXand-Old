using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.DistributedServices.Process
{
    public abstract class ProcessManagerBase<TStartProcessRequest, TCompleteTaskRequest> : 
        IProcessManager<TStartProcessRequest, TCompleteTaskRequest>
        where TStartProcessRequest : IStartProcessRequest
        where TCompleteTaskRequest : ICompleteTaskRequest        
    {
        public ProcessActionResult CompleteTask(ICompleteTaskRequest completeTaskRequest)
        {
            return CompleteTask((TCompleteTaskRequest)completeTaskRequest);
        }

        public abstract ProcessActionResult CompleteTask(TCompleteTaskRequest taskInformation);

        public ProcessActionResult StartProcess(IStartProcessRequest startProcessRequest)
        {
            return StartProcess((TStartProcessRequest)startProcessRequest);
        }

        public abstract ProcessActionResult StartProcess(TStartProcessRequest startProcessRequest);
    }
}
