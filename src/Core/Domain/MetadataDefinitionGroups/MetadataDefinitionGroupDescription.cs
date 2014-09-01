using System;
using Lacjam.Framework.Exceptions;

namespace Lacjam.Core.Domain.MetadataDefinitionGroups
{
    public class MetadataDefinitionGroupDescription
    {
        public MetadataDefinitionGroupDescription(string description)
        {

            if (String.IsNullOrWhiteSpace(description))
                throw new InvariantGuardFailureException();

            Description = description;
        }

        public string Description { get; private set; }
    }
}