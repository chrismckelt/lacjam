using System.Collections.Generic;
using Lacjam.Framework.Events;

namespace Lacjam.Framework.Handlers
{
    public interface IEventPublisher
    {
        void Publish(IEnumerable<IEvent> @events);
    }
}