using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lacjam.Framework.Converters
{
    public class DateTimeAsAuShortDateConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var t = JToken.FromObject(value);
            var o = (JValue)t;
            o.Value = ((System.DateTime)o.Value).ToString("dd/MM/yyyy");
            o.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new Exception("Not called due to CanRead: false.");
        }

        public override bool CanConvert(Type objectType){return objectType == typeof(System.DateTime);}

        public override bool CanRead {get{return false;}}

        public override bool CanWrite {get { return true; }}
    }
}
