using System;

namespace Lacjam.Framework.Events
{

    [AttributeUsage(AttributeTargets.Method)]
    public class ImmediateDispatchAttribute : Attribute
    {
        
    }
}