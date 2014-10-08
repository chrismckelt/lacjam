using System;

namespace Lacjam.Framework.Exceptions
{
    public class InvariantGuardFailureException : ArgumentNullException
    {
        public InvariantGuardFailureException()
        {
        }

        public InvariantGuardFailureException(string arg) : base(arg)
        {
        }
    }
}

