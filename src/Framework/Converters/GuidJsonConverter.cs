using System;
using System.Diagnostics;
using Newtonsoft.Json;
using Lacjam.Framework.Utilities;

namespace Lacjam.Framework.Converters
{
    public class GuidJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Guid guid = Guid.Empty;

            if (Guid.TryParse(Convert.ToString(value), out guid))
            {
                serializer.Serialize(writer, guid);
            }
            else
            {
                serializer.Serialize(writer, (Guid)value);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var uuid = serializer.Deserialize<string>(reader);
            if (objectType == typeof (Guid?) && string.IsNullOrWhiteSpace(uuid))
            {
                return null;
            }
            var guid = uuid.ToGuid();
            return guid;
        }

        [DebuggerStepThrough]
        public override bool CanConvert(Type objectType)
        {
            return typeof(Guid) == objectType || typeof(Guid?) == objectType;
        }
    }

 
}