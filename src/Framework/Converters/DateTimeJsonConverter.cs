using System;
using System.Diagnostics;
using System.Globalization;
using Newtonsoft.Json;
using Lacjam.Framework.Extensions;
using Lacjam.Framework.Time;

namespace Lacjam.Framework.Converters
{
    public class DateTimeJsonConverter : JsonConverter
    {
        private static readonly string[] DateFormats =
        {
            "s",
            "u",
            "yyyy-MM-dd hh:mm:ss",
            "yyyy-MM-dd hh:mm",
            "yyyy-MM-dd"
        };

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dateTime = (System.DateTime?) value;
            if (dateTime.HasValue)
            {
                serializer.Serialize(writer,dateTime.Value.DateTimeUtcToUnixTime());
            }
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
			if (reader.ValueType == typeof(System.DateTime))
			{
				return reader.Value;
			}

			var dateTimeString = serializer.Deserialize<string>(reader);
			if (objectType == typeof(System.DateTime?) && string.IsNullOrWhiteSpace(dateTimeString))
			{
				return null;
			}

            long epochTime;
            System.DateTime result;
            if (long.TryParse(dateTimeString, out epochTime))
                return epochTime.FromUnixTimeToDateTimeUtc();
            if (System.DateTime.TryParseExact(dateTimeString, DateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None,out result))
                return result;
            throw new FormatException("Could not parse date: " + dateTimeString);
        }

        [DebuggerStepThrough]
        public override bool CanConvert(Type objectType)
        {
            return typeof(System.DateTime) == objectType || typeof(System.DateTime?) == objectType;
        }
    }
}