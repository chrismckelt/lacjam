using System;
using Lacjam.Framework.Exceptions;
using Lacjam.Framework.Serialization;

namespace Lacjam.Framework.Events
{
    public static class DynamicEventValidationExtensions
    {

        public static void Validate(this DynamicEvent @event, IEventValidator validator)
        {
            if (String.IsNullOrWhiteSpace(@event.EventType))
                return;

            var type = EventTypeCache.GetType(@event.EventType);

            if(type == null)
                throw new IgnoredEventException();

            var item = EventDescriptorExtensions.ForceConvertToStrongEventType(@event.EventType,@event, ValidationSerializerSettings.Instance);

            validator.DynamicInvokeValidate(type, item);

        }

    }

    public interface IEventValidator
    {
        void ValidateEvent<T>(T @event) where T : class;
        void DynamicInvokeValidate(Type eventType, IEvent @event);
    }
}