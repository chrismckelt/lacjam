using Lacjam.Framework.Events;
using Lacjam.Framework.FP;

namespace Lacjam.Framework.Dispatchers
{
    public interface ISequenceEventSelector
    {
        IMaybe<SequencedEvent> GetSequencedEvent();
    }
}