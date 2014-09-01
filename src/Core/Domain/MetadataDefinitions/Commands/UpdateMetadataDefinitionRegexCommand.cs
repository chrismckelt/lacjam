using System;
using Lacjam.Framework.Commands;

namespace Lacjam.Core.Domain.MetadataDefinitions.Commands
{
    public class UpdateMetadataDefinitionRegexCommand : ICommand
    {
        public UpdateMetadataDefinitionRegexCommand(Guid identity, string regex)
        {
            Identity = identity;
            Regex = regex;
        }

        public Guid Identity { get; private set; }
        public string Regex { get; private set; }
    }
}