using Lacjam.Framework.Commands;

namespace Lacjam.Framework.Handlers
{
    public interface ICommandHandler<T> where T : ICommand
    {
        void Handle(T command);
    }
}
