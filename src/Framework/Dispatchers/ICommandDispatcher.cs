using Lacjam.Framework.Commands;

namespace Lacjam.Framework.Dispatchers
{
    public interface ICommandDispatcher
    {
        void Dispatch<T>(T command) where T : ICommand;
    }
}
