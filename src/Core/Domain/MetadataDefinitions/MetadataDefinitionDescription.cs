using System;
using Lacjam.Framework.Exceptions;

namespace Lacjam.Core.Domain.MetadataDefinitions
{
    public class MetadataDefinitionDescription
    {

        protected MetadataDefinitionDescription()
        {
        }

        public MetadataDefinitionDescription(string name)
        {

            if (String.IsNullOrWhiteSpace(name))
                throw new InvariantGuardFailureException();

            Name = name;
        }

        public string Name { get; private set; }
    }
}