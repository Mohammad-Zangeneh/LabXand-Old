using LabXand.Core.ExceptionManagement;
using LabXand.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.DistributedServices.ClientProxy
{
    public abstract class ClientProxyBase<TService> : InvokableClass
    {
        public ClientProxyBase(IExceptionHandler exceptionHandler)
        {
            ExceptionHandler = exceptionHandler;
        }
        ChannelFactory<TService> channel;
        TService proxy;
        protected ChannelFactory<TService> Channel
        {
            get
            {
                if (channel == null)
                {
                    CreateChannel();
                }
                return channel;
            }
        }
        protected IExceptionHandler ExceptionHandler { get; private set; }
        private void CreateChannel()
        {
            channel = new ChannelFactory<TService>("*");
            channel.Faulted += channel_Faulted;
            Initialize(channel);
            proxy = channel.CreateChannel();
        }

        protected abstract void Initialize(ChannelFactory<TService> channel);

        void channel_Faulted(object sender, EventArgs e)
        {
            ((ICommunicationObject)sender).Abort();
            CreateChannel();
        }

        protected TService Proxy
        {
            get
            {
                if (Channel.State != CommunicationState.Opened)
                {
                    Channel.Open();
                }
                return proxy;
            }
        }

        public override void HandleException(Exception ex)
        {
            ExceptionHandler.Handle(ex);
        }
    }
}