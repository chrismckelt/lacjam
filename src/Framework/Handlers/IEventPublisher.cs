using System.Collections.Generic;
using Structerre.MetaStore.Framework.Events;

namespace Structerre.MetaStore.Framework.Handlers
{
    public interface IEventPublisher
    {
        void Publish(IEnumerable<IEvent> @events);
    }
}