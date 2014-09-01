using System;
using Lacjam.Framework.Exceptions;

namespace Lacjam.Core.Domain.MetadataDefinitions
{

    public class MetadataDefinitionName
    {
       
        protected MetadataDefinitionName()
        {
        }

        public MetadataDefinitionName(string name)
        {

            if(String.IsNullOrWhiteSpace(name))
                throw new InvariantGuardFailureException();

            Name = name;
        }

        public string Name { get; private set; }
    }
}
