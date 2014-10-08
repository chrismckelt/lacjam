using System;
using Lacjam.Framework.Exceptions;

namespace Lacjam.Core.Domain.MetadataDefinitions
{
    public class MetadataDefinitionDescription
    {

        protected MetadataDefinitionDescription()
        {
        }

        public MetadataDefinitionDescription(string description)
        {

            if (String.IsNullOrWhiteSpace(description))
                throw new InvariantGuardFailureException("description");

            Description = description;
        }

        public string Description { get; private set; }
    }
}