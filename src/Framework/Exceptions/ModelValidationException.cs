using System;
using System.Runtime.Serialization;

namespace Lacjam.Framework.Exceptions
{
    public class ModelValidationException : Exception
    {
        public ModelValidationException()
        {
        }

        public ModelValidationException(string message) : base(message)
        {
        }

        public ModelValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ModelValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
