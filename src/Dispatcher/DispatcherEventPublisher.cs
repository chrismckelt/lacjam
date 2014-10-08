using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Structerre.MetaStore.Core.Infrastructure.Ioc;
using Structerre.MetaStore.Framework.Dispatchers;
using Structerre.MetaStore.Framework.Events;
using Structerre.MetaStore.Framework.FP;
using Structerre.MetaStore.Framework.Handlers;
using Structerre.MetaStore.Framework.Logging;

namespace Structerre.MetaStore.Dispatcher
{
    public class DispatcherEventPublisher : IDispatcherEventPublisher
    {
        public DispatcherEventPublisher(IHandlerSequenceRespository handlerSequenceRespository, ILogWriter logger)
        {
            _handlerSequenceRespository = handlerSequenceRespository;
            _logger = logger;
        }

        public void Publish<T>(T @event, long seq = -1) where T : IEvent
        {
            try
            {
                DispatchToHandlers(@event);

                if (seq > 0)
                    _handlerSequenceRespository.Save(seq, "Dispatcher");

            }
            catch (Exception e)
            {
                _logger.Error(EventIds.Dispatcher, "RunHandlers", e);
                throw;
            }
        }

        private void DispatchToHandlers(IEvent @event)
        {
            var type = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
            var handlers = WindsorAccessor.Instance.Container.ResolveAll(type);
           // var handlersToDispatchTo = handlers.Cast<object>().Where(handler => CanAcceptDispatch(@event, handler));
            foreach (var handler in handlers)
                Dispatch(@event, handler);
        }

        private static void Dispatch(IEvent @event, Object handler)
        {
            var methods = handler.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public)
                                           .Where(x => x.Name.Equals("Handle"))
                                           .Where(x => !x.GetCustomAttributes(typeof(ImmediateDispatchAttribute)).Any());

            methods.FirstOrDefault(method => method.GetParameters().Any(param => param.ParameterType == @event.GetType()))
                   .ToMaybe()
                   .Foreach(x =>
                   {
                       x.Invoke(handler, BindingFlags.Public | BindingFlags.Instance, null, new[] { @event }, CultureInfo.CurrentCulture);
                   });
        }

        private static bool CanAcceptDispatch(IEvent @event, Object handler)
        {
            var methods = handler.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public)
                                           .Where(x => x.Name.Equals("Handle"))
                                           .Where(x => !x.GetCustomAttributes(typeof(ImmediateDispatchAttribute)).Any());

            return methods.Any(method => method.GetParameters().Any(param => param.ParameterType == @event.GetType()));
        }

        private readonly IHandlerSequenceRespository _handlerSequenceRespository;
        private readonly ILogWriter _logger;

    }
}
