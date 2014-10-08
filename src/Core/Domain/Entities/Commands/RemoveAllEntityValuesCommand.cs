using System;
using Lacjam.Framework.Commands;

namespace Lacjam.Core.Domain.Entities.Commands
{
    public class RemoveAllEntityValuesCommand : ICommand
    {
        public RemoveAllEntityValuesCommand(Guid aggregateIdentity)
        {
            AggregateIdentity = aggregateIdentity;

        }
        public Guid AggregateIdentity { get; private set; }
    }
}