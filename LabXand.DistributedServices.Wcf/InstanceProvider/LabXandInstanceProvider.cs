using LabXand.Infrastructure.Data;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace LabXand.DistributedServices.Wcf
{
    public class LabXandInstanceProvider : IInstanceProvider
    {
        #region Members

        readonly Type _serviceType;
        readonly IContainer _container;
        string instanceKey = string.Empty;
        #endregion

        #region Constructor

        /// <summary>
        /// Create a new instance of unity instance provider
        /// </summary>
        /// <param name="serviceType">The service where we apply the instance provider</param>
        public LabXandInstanceProvider(Type serviceType,IContainer container)
        {
            if (serviceType == null)
                throw new ArgumentNullException("serviceType");
            
            _serviceType = serviceType;
            _container = container;

            if (OperationContext.Current != null)
            {
                IDataContext dbContext = _container.GetInstance<IDataContext>();
                if (dbContext != null)
                {
                    //instanceKey = dbContext.InstanceKey;
                    OperationContext.Current.Extensions.Add(new OperationContainerExtension(dbContext));
                    OperationContext.Current.OperationCompleted += Current_OperationCompleted;
                    OperationContext.Current.Channel.Faulted += Channel_Faulted;
                }
            }
        }

        void Channel_Faulted(object sender, EventArgs e)
        {
            IDataContext dbContext = GetDbContext();
            if (dbContext != null)
            {
                dbContext.Dispose();
                GC.Collect();
            }
        }

        private void Current_OperationCompleted(object sender, EventArgs e)
        {
            IDataContext dbContext = GetDbContext();
            if (dbContext != null)
            {
                dbContext.Dispose();
                GC.Collect();
            }
        }

        protected IDataContext GetDbContext()
        {
            if (OperationContext.Current != null)
            {
                var operationContainerExtension = OperationContext.Current.Extensions.OfType<OperationContainerExtension>().FirstOrDefault(e => e.ContextKey == instanceKey);
                if (operationContainerExtension != null)
                {
                    return operationContainerExtension.CurrentDbContext;
                }
                return null;
            }
            else
                return null;
        }

        #endregion

        #region IInstance Provider Members

        /// <summary>
        /// <see cref="System.ServiceModel.Dispatcher.IInstanceProvider"/>
        /// </summary>
        /// <param name="instanceContext"><see cref="System.ServiceModel.Dispatcher.IInstanceProvider"/></param>
        /// <param name="message"><see cref="System.ServiceModel.Dispatcher.IInstanceProvider"/></param>
        /// <returns><see cref="System.ServiceModel.Dispatcher.IInstanceProvider"/></returns>
        public object GetInstance(InstanceContext instanceContext, System.ServiceModel.Channels.Message message)
        {
            //This is the only call to UNITY container in the whole solution
            try
            {
                return _container.GetInstance(_serviceType);
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }
        /// <summary>
        /// <see cref="System.ServiceModel.Dispatcher.IInstanceProvider"/>
        /// </summary>
        /// <param name="instanceContext"><see cref="System.ServiceModel.Dispatcher.IInstanceProvider"/></param>
        /// <returns><see cref="System.ServiceModel.Dispatcher.IInstanceProvider"/></returns>
        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        /// <summary>
        /// <see cref="System.ServiceModel.Dispatcher.IInstanceProvider"/>
        /// </summary>
        /// <param name="instanceContext"><see cref="System.ServiceModel.Dispatcher.IInstanceProvider"/></param>
        /// <param name="instance"><see cref="System.ServiceModel.Dispatcher.IInstanceProvider"/></param>
        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            if (instance is IDisposable)
                ((IDisposable)instance).Dispose();
        }

        #endregion

    }

    public class OperationContainerExtension : IExtension<OperationContext>
    {
        public OperationContainerExtension(IDataContext dbContext)
        {
            this.CurrentDbContext = dbContext;
            //this.ContextKey = dbContext.InstanceKey;
        }

        public IDataContext CurrentDbContext
        {
            get;
            private set;
        }

        public string ContextKey
        {
            get;
            private set;
        }

        public void Attach(OperationContext owner)
        {
        }

        public void Detach(OperationContext owner)
        {
        }
    }
}
