using System;

namespace LabXand.Security.Core
{

    [Serializable]
    public class UnAuthenticatedException : Exception
    {
        const string MESSAGE = "You are not authenticated.";
        public UnAuthenticatedException() : base(string.Format(MESSAGE))
        {
        }
        public UnAuthenticatedException(Exception inner) : base(string.Format(MESSAGE), inner)
        {
        }
        protected UnAuthenticatedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
