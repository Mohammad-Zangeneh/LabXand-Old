using LabXand.Security.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Security.Wcf
{
    public class SecurityOperationContext<TUserContext> : IExtension<OperationContext>
        where TUserContext : class,IUserContext
    {
        private readonly IDictionary<string, object> items;
        private const string userContextKey = "UserContext";

        protected SecurityOperationContext()
        {
            items = new Dictionary<string, object>();
            SetUserContext();
        }

        public IDictionary<string, object> Items
        {
            get { return items; }
        }

        protected void SetUserContext()
        {
            if (ServiceSecurityContext.Current != null && ServiceSecurityContext.Current.PrimaryIdentity != null)
            {
                Items[userContextKey] = InitializeUserContext(ServiceSecurityContext.Current.PrimaryIdentity);
            }
        }

        protected virtual TUserContext InitializeUserContext(IIdentity identity)
        {
            throw new NotImplementedException();
        }
        public TUserContext UserContext
        {
            get
            {
                if (Items.ContainsKey(userContextKey))
                    return Items[userContextKey] as TUserContext;
                return null;
            }
        }

        public static SecurityOperationContext<TUserContext> Current
        {
            get
            {
                SecurityOperationContext<TUserContext> context = OperationContext.Current.Extensions.Find<SecurityOperationContext<TUserContext>>();
                if (context == null)
                {
                    context = new SecurityOperationContext<TUserContext>();
                    OperationContext.Current.Extensions.Add(context);
                }
                return context;
            }
        }

        public void Attach(OperationContext owner)
        {

        }

        public void Detach(OperationContext owner)
        {

        }
    }
}
