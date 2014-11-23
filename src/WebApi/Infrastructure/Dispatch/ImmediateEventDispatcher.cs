using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Lacjam.Core.Infrastructure.Ioc;
using Lacjam.Framework.Events;
using Lacjam.Framework.Extensions;
using Lacjam.Framework.FP;
using Lacjam.Framework.Handlers;

namespace Lacjam.WebApi.Infrastructure.Dispatch
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
