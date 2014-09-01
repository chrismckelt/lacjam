using System.Collections.Generic;
using Lacjam.Framework.Events;

namespace Lacjam.Framework.Model
{
    public interface IAggregateRoot : IEntity
    {
        IEnumerable<IEvent> GetUncommitedChanges();
        void MarkChangesCommited();
        int GetVersion();
    }
}
