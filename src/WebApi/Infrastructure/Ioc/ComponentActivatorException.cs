using System;

namespace Lacjam.WebApi.Infrastructure.Ioc
{
    public class ComponentActivatorException : Exception
    {
        public ComponentActivatorException(string message, Exception innerException)
            : base(message, innerException)
        {}
    }
}