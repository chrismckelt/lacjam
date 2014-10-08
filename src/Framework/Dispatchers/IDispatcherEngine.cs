using System;

namespace Lacjam.Framework.Dispatchers
{
    public interface IDispatcherEngine
    {
        void Process(Func<bool> terminat, bool exitAtEndOfStream, bool includeImmediate);
    }
}