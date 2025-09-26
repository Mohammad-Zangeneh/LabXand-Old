using LabXand.DomainLayer;
using LabXand.Logging.Core;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace LabXand.DistributedServices.Wcf
{
    public class LabXandErrorHandler : IErrorHandler, IServiceBehavior
    {
        ILogger _logger;
        public LabXandErrorHandler(ILogger logger)
        {
            _logger = logger;
        }
        public bool HandleError(Exception exception)
        {
            //_logger.Log(ExceptionHelper.GetDetails(exception));
            return false;
        }

        public void ProvideFault(Exception error, System.ServiceModel.Channels.MessageVersion version, ref System.ServiceModel.Channels.Message fault)
        {
            //string message = "Server error encountered. All details have been logged.";
            string message = error.Message;
            FaultException faultException = null;
            FaultCode faultCode = null;
            if (error is ArgumentException)
            {
                faultCode = new FaultCode("ArgumentException");
            }

            if (error is ViolationServiceRuleException)
            {
                faultCode = new FaultCode("ViolationServiceRuleException");
            }

            if (faultCode == null)
                faultException = new FaultException(message);
            else
                faultException = new FaultException(message, faultCode);

            MessageFault messageFault = faultException.CreateMessageFault();

            fault = Message.CreateMessage(version, messageFault, faultException.Action);
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            return;
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher disp in serviceHostBase.ChannelDispatchers)
                disp.ErrorHandlers.Add(new LabXandErrorHandler(_logger));
        }

        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        { }
    }
}
