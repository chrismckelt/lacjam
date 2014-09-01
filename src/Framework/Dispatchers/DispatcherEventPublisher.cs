using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.Windsor;
using Iesi.Collections;
using Newtonsoft.Json;
using Lacjam.Framework.Events;
using Lacjam.Framework.Handlers;
using Lacjam.Framework.Logging;
using Lacjam.Framework.Utilities;

namespace Lacjam.Framework.Dispatchers
{
    public class DispatcherEventPublisher : IDispatcherEventPublisher
    {
        public static List<Tuple<Type, MethodInfo, Type>> HandlerTuples; // handler, method to hit, event type param
        public static ISet Handlers;
        private readonly IHandlerSequenceRespository _handlerSequenceRespository;
        private readonly ILogWriter _logger;
        private readonly IWindsorContainer _container;


        public DispatcherEventPublisher(IHandlerSequenceRespository handlerSequenceRespository, ILogWriter logger,
            IWindsorContainer container)
        {
            _handlerSequenceRespository = handlerSequenceRespository;
            _logger = logger;
            _container = container;
        }

        public void Publish<T>(T @event, long seq = -1, bool runImmediately = true) where T : IEvent
        {
            try
            {
                _logger.Debug(EventIds.Dispatcher, "Dispatching " + seq);
                _logger.Debug(EventIds.Dispatcher, "Dispatching " + JsonConvert.SerializeObject(@event));

                CheckHandlers();
                DispatchToHandlers(@event, runImmediately);

                if (seq > 0 || !runImmediately)
                    _handlerSequenceRespository.Save(seq, "Dispatcher");
            }
            catch (Exception e)
            {
                _logger.Error(EventIds.Dispatcher, "RunHandlers", e);
                throw;
            }
        }

        private void CheckHandlers()
        {
            if (HandlerTuples != null) return;

            HandlerTuples = new List<Tuple<Type, MethodInfo, Type>>();

            var eventTypes = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.GetName().Name.Contains("Lacjam"))
                .SelectMany(assembly => assembly.GetExportedTypes())
                .Where(type => !type.IsInterface)
                .Where(x => typeof(IEvent).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                ;

            foreach (var et in eventTypes)
            {
                var handlerTypes = typeof(IEventHandler<>).MakeGenericType(et);

                if (Handlers == null) Handlers = new ListSet();
                var arr = _container.ResolveAll(handlerTypes);
                foreach (var ar in arr)
                {
                    object ar1 = ar;
                    object ar2 = ar;
                    FinallyGuarded.Apply(() => Handlers.Add(ar1),
                        () => _logger.Warn(EventIds.Warn, "INVALID eventhandler - " + ar2));
                }

                foreach (var handler in Handlers)
                {
                    var mis = handler.GetType().GetMethods(
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .Where(x => x.Name == "Handle" && x.GetParameters().Length > 0);

                    foreach (var mi in mis)
                    {
                        var eventType = mi.GetParameters()[0].ParameterType;
                        HandlerTuples.Add(new Tuple<Type, MethodInfo, Type>(handler.GetType(), mi, eventType));
                    }
                }
            }
        }

        private void DispatchToHandlers(IEvent @event, bool runImmediately = true)
        {
            var type = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
            var handlers = _container.ResolveAll(type);

            foreach (var handler in handlers)
            {
                var ht = handler.GetType();
                var et = @event.GetType();
                var tuple = HandlerTuples.First(x => x.Item1 == ht && x.Item3 == et);

                if (!tuple.Item1.GetCustomAttributes(typeof(ImmediateDispatchAttribute)).Any() == runImmediately)
                {    // class level override
                    if (!tuple.Item2.GetCustomAttributes(typeof(ImmediateDispatchAttribute)).Any() == runImmediately)
                        continue; //method level
                }

                if (ht != tuple.Item1 || et != tuple.Item3) continue;
                try
                {
                    var dyn = (dynamic)handler;
                    dyn.Handle((dynamic)@event);
                }
                catch (Exception ex)
                {
                    _logger.Error(EventIds.Dispatcher, handler.GetType().Name, ex);
                    throw;
                }
            }

        }
    }
}