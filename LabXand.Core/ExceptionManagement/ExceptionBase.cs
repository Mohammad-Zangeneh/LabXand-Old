using System;

namespace LabXand.Core
{

    [Serializable]
    public class ExceptionBase : Exception
    {
        public ExceptionBase() { }
        public ExceptionBase(string message) : base(message) { }
        public ExceptionBase(string message, Exception inner) : base(message, inner) { }
        protected ExceptionBase(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
