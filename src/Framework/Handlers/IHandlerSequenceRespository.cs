using Lacjam.Framework.FP;

namespace Lacjam.Framework.Handlers
{
    public interface IHandlerSequenceRespository
    {
        void Save(long eventSequence, string name);
        IMaybe<long> GetEventSequence(string name);
    }
}