using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabXand.Core.Infrastructure
{
    public abstract class InvokableClass
    {
        protected T Invoke<T>(Func<T> method)
        {
            try
            {
                return method.Invoke();
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return default(T);
            }
        }

        protected void Invoke(Action method)
        {
            try
            {
                method.Invoke();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public abstract void HandleException(Exception ex);
    }
}
