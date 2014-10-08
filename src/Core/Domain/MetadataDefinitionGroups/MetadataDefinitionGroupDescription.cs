using System;
using Lacjam.Framework.Exceptions;

namespace Lacjam.Core.Domain.MetadataDefinitionGroups
{
    public class MetadataDefinitionGroupDescription
    {
        public MetadataDefinitionGroupDescription(string description)
        {
            Description = description;
        }

        public string Description { get; private set; }
    }
}