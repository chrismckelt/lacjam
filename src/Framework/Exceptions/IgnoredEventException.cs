using System;

namespace Lacjam.Framework.Exceptions
{
    public class IgnoredEventException : Exception
    {
        public IgnoredEventException() : base("Ignored Event")
        {
            
        }
    }
}