using System;
using System.Diagnostics;
using Newtonsoft.Json;
using Lacjam.Framework.Utilities;

namespace Lacjam.Framework.Converters
{
    public class GuidToUuidJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var guid = (Guid) value;
            var uuid = guid.ToUuid();
            serializer.Serialize(writer,uuid);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var gs = serializer.Deserialize<string>(reader);
            if (string.IsNullOrEmpty(gs)) 
                return gs;

            if (gs.Length <= 16) 
                return gs;

            Guid g;
            return Guid.TryParse(gs, out g) ? g.ToUuid() : gs;
        }

        [DebuggerStepThrough]
        public override bool CanConvert(Type objectType)
        {
            return typeof(Guid) == objectType || typeof(Guid?) == objectType;
        }
    }
}