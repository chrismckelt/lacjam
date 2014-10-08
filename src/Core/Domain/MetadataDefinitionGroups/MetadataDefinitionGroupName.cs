using System;
using Lacjam.Framework.Exceptions;

namespace Lacjam.Core.Domain.MetadataDefinitionGroups
{
    public class MetadataDefinitionGroupName
    {
        public MetadataDefinitionGroupName(string name)
        {

            if (String.IsNullOrWhiteSpace(name))
                throw new InvariantGuardFailureException("name");

            Name = name;
        }

        public string Name{ get; private set; }
    }
}
