using System;
using System.Collections.Generic;
using Lacjam.Framework.Commands;

namespace Lacjam.Core.Domain.Entities.Commands
{
    public class UpdateEntityValuesCommand : ICommand
    {
        public UpdateEntityValuesCommand(Guid aggregateIdentity, HashSet<EntityValueBag> definitionValues)
        {
            AggregateIdentity = aggregateIdentity;
            DefinitionValues = definitionValues;
        }

        public Guid AggregateIdentity { get; private set; }
        public HashSet<EntityValueBag> DefinitionValues { get; private set; }
    }
}