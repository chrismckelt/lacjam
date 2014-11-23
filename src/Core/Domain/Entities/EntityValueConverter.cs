using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHibernate.Util;

namespace Lacjam.Core.Domain.Entities
{
    public class EntityValueConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var val = (IValue)(value);
            var stream = val.GetStream().ToList();
            var multiple = stream.Count > 1;

            writer.WriteStartObject();
            writer.WritePropertyName("multiple");
            serializer.Serialize(writer, multiple);
            writer.WritePropertyName("value");
            serializer.Serialize(writer, stream);
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var properties = jsonObject.Properties().ToList();
            var multiple = (bool)properties[0].Value;

            if (!multiple)
            {
                var array = (JContainer) (properties[1].Value);
                return new SingleValue(array.Select(x=> x.Value<string>()).FirstOrDefault());
            }

            return new MultiValue(properties[1].Value.Select(x=> x.Value<string>()));
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IValue);
        }

        public override bool CanWrite
        {
            get { return true; }
        }
    }
}