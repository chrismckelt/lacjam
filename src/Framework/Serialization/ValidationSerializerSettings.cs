using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Lacjam.Framework.Converters;

namespace Lacjam.Framework.Serialization
{
    public static class ValidationSerializerSettings
    {
        public static readonly JsonSerializerSettings Instance = GetJsonSerializerSettings();

        public static readonly CultureInfo DefaultCulture = new CultureInfo("en-AU");

        private static JsonSerializerSettings GetJsonSerializerSettings()
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Culture = DefaultCulture
            };
            settings.Converters.Add(new DateTimeJsonConverter());
            settings.Converters.Add(new GuidJsonConverter());
            settings.Converters.Add(new EnumJsonConverter());
            return settings;
        }

        public static void AddConverter(JsonConverter converter)
        {
            if (!Instance.Converters.Contains(converter))
            {
                Instance.Converters.Add(converter);
            }
        }
    }
}