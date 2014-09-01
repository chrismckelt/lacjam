using System;
using Newtonsoft.Json;
namespace Lacjam.Framework.Events
{
    public static class EventDescriptorExtensions
    {
        public static IEvent ForceConvertToStrongEventType(string eventType, DynamicEvent dynamicEvent, JsonSerializerSettings settings)
        {
            var type = Type.GetType(eventType);
            return (IEvent)JsonConvert.DeserializeObject(dynamicEvent.Json, type, settings);
        }
    }
}