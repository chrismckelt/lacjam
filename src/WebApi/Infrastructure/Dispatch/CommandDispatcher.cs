using System;
using System.Linq;
using Lacjam.Core.Infrastructure.Ioc;
using Lacjam.Core.Infrastructure.Ioc.Convo;
using Lacjam.Framework.Commands;
using Lacjam.Framework.Dispatchers;
using Lacjam.Framework.Extensions;
using Lacjam.Framework.Handlers;

namespace Lacjam.WebApi.Infrastructure.Dispatch
{
    public class CommandDispatcher : ICommandDispatcher
    {

        public void Dispatch<T>(T command) where T : ICommand
        {
            var handlers = WindsorAccessor.Instance.Container.ResolveAll<ICommandHandler<T>>();

            if (handlers == null || !handlers.Any())
                throw new MissingHandlerRegistrationException();

            handlers.Each(x => x.Handle(command));


        }
    }

    public class MissingHandlerRegistrationException : Exception
    {
    }
}