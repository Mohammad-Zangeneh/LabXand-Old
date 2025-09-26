using LabXand.Logging.Core;
using System;
using System.ServiceModel.Configuration;

namespace LabXand.DistributedServices.Wcf
{
    public class ErrorHandlerExtention : BehaviorExtensionElement
    {
        ILogger _logger;
        public ErrorHandlerExtention(ILogger logger)
        {
            _logger = logger;
        }
        public override Type BehaviorType
        {
            get { return typeof(LabXandErrorHandler); }
        }

        protected override object CreateBehavior()
        {
            return new LabXandErrorHandler(_logger);
        }
    }
}
