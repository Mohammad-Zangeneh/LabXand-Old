using LabXand.Logging.Core;
using StructureMap;
using System;
using System.ServiceModel;

namespace LabXand.DistributedServices.Wcf.Host
{
    public class LabXandServiceHostBase : ServiceHost
    {
        IContainer _container;
        ILogger _logger;

        public LabXandServiceHostBase(IContainer container, Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            _container = container;
            this._logger = _container.GetInstance<ILogger>();
        }

        protected override void OnOpening()
        {
            Description.Behaviors.Add(new LabXandErrorHandler(_logger));
            base.OnOpening();
        }
    }
}
