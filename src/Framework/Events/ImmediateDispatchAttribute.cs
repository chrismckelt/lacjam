using System;

namespace Lacjam.Framework.Events
{

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class ImmediateDispatchAttribute : Attribute
    {
        
    }
}