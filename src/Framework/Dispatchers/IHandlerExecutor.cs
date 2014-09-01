using Lacjam.Framework.Events;

namespace Lacjam.Framework.Dispatchers
{
    public interface IHandlerExecutor
    {
        void HandleSequencedEvent(SequencedEvent sequencedEvent, bool runImmediately =  true);
    }
}