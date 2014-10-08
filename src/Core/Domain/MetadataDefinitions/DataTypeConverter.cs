using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lacjam.Core.Domain.MetadataDefinitions
{
    public class DataTypeConverter : JsonConverter
    {
        private const string DATA_TYPE_PROPERTY = "dataType";
        private const string DATA_REGEX_PROPERTY = "regex";

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var datatype = (IDataType)(value);
            var tag = datatype.Tag;

            var regexType = datatype as RegexDataType;

            writer.WriteStartObject();
            writer.WritePropertyName(DATA_TYPE_PROPERTY);
            serializer.Serialize(writer, tag);
            if (regexType != null)
            {
                writer.WritePropertyName(DATA_REGEX_PROPERTY);
                serializer.Serialize(writer, regexType.Regex);    
            }
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var tag = (string)jsonObject.Property(DATA_TYPE_PROPERTY).Value;
            return DataTypeBuilder.Create(tag);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IDataType);
        }

        public override bool CanWrite
        {
            get { return true; }
        }
    }
}