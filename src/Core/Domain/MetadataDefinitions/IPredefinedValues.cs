using System.Collections.Generic;

namespace Lacjam.Core.Domain.MetadataDefinitions
{
    public interface IPredefinedValues
    {
        IEnumerable<string> Set { get; } 
    }
}