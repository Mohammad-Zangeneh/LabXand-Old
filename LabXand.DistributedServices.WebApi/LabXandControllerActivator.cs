using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace LabXand.DistributedServices.WebApi
{
    public class LabXandControllerActivator : IHttpControllerActivator
    {
        private readonly IContainer container;

        public LabXandControllerActivator(IContainer container)
        {
            this.container = container;
        }

        public IHttpController Create(
            HttpRequestMessage request,
            HttpControllerDescriptor controllerDescriptor,
            Type controllerType)
        {
            return container.GetInstance(controllerType) as IHttpController;
        }
    }
}