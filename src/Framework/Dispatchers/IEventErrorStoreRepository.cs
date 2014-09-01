using Lacjam.Framework.Events;

namespace Lacjam.Framework.Dispatchers
{
    public interface IEventErrorStoreRepository
    {
        void Transfer(EventDescriptor @event);
        void Transfer(EventErrorDescriptor @event);
        void Delete(long seq);
        bool PushBack(long seq);
        EventErrorDescriptor Get(long seq);
    }
}