using System;
using Lacjam.Framework.Commands;

namespace Lacjam.Core.Domain.Entities.Commands
{
    public class DeleteEntityCommand : ICommand
    {
        public DeleteEntityCommand(Guid identity)
        {
            Identity = identity;
        }

        public Guid Identity { get; private set; }
    }
}