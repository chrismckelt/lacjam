using System;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Lacjam.Framework.Converters
{
    public class InlineJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(((InlineJson)value).Data);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            dynamic json = serializer.Deserialize(reader);
            if (json == null)
                return null;
            var str = json.ToString();
            return new InlineJson {Data = str};
        }

        [DebuggerStepThrough]
        public override bool CanConvert(Type objectType)
        {
            return typeof (InlineJson) == objectType;
        }
    }
}