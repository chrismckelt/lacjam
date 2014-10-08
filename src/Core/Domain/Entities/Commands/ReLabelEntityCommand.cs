using System;
using Lacjam.Framework.Commands;

namespace Lacjam.Core.Domain.Entities.Commands
{
    public class ReLabelEntityCommand : ICommand
    {
        public ReLabelEntityCommand(Guid aggregateIdentity, EntityName name)
        {
            AggregateIdentity = aggregateIdentity;
            Name = name;
        }

        public Guid AggregateIdentity { get; private set; }
        public EntityName Name { get; private set; }
    }
}