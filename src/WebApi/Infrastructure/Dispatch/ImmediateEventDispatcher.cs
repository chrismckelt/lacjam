using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Structerre.MetaStore.Core.Infrastructure.Ioc;
using Structerre.MetaStore.Framework.Events;
using Structerre.MetaStore.Framework.Extensions;
using Structerre.MetaStore.Framework.FP;
using Structerre.MetaStore.Framework.Handlers;

namespace Structerre.MetaStore.WebApi.Infrastructure.Dispatch
{
    public class ImmediateEventDispatcher : IEventPublisher
    {
        public void Publish(IEnumerable<IEvent> @events)
        {
            @events.Each(DispatchToHandlers);
        }



        private void DispatchToHandlers(IEvent @event)
        {
            
            var type = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
            var handlers = WindsorAccessor.Instance.Container.ResolveAll(type);
           
            foreach (var handler in handlers)
            {
                if (CanAcceptDispatch(@event, handler))
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
            }
        }

        private static bool CanAcceptDispatch(IEvent @event, Object handler)
        {
            var methods = handler.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public)
                                           .Where(x => x.Name.Equals("Handle"))
                                           .Where(x => x.GetCustomAttributes(typeof(ImmediateDispatchAttribute)).Any());

            foreach (var method in methods)
            {
                foreach (var parameterInfo in method.GetParameters())
                {
                    if (parameterInfo.ParameterType == @event.GetType()) return true;
                }
                   
            }

            return false;
        }
    }
}
