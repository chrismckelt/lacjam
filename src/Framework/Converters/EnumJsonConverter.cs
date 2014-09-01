using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;

namespace Lacjam.Framework.Converters
{
    public class EnumJsonConverter : JsonConverter
    {
        public static string GetDescriptionFromEnumValue(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            if (field == null)
                return null;

            var attribute = field
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            var e = (Enum)value;
            var description = GetDescriptionFromEnumValue(e);
            if (description != null)
                writer.WriteValue(description);
            else
                writer.WriteValue(value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead
        {
            get { return false; }
        }
        public override bool CanWrite
        {
            get { return true; }
        }

        [DebuggerStepThrough]
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsEnum;
        }
    }
}