using System;
using System.Runtime.Serialization;

namespace LabXand.DomainLayer.Core
{

    [Serializable]
    public class InvalidDomainException : Exception
    {
        public InvalidDomainException() { }
        public InvalidDomainException(string message) : base(message) { }
        public InvalidDomainException(string message, Exception inner) : base(message, inner) { }
        protected InvalidDomainException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context) { }
    }    
}
