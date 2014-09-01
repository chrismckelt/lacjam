using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Lacjam.Framework.Converters;

namespace Lacjam.Framework.Serialization
{
    public static class MetaStoreSerializerSettings
    {
        public static readonly JsonSerializerSettings Instance = GetJsonSerializerSettings();

        private static readonly CultureInfo DefaultCulture = new CultureInfo("en-AU");

        private static JsonSerializerSettings GetJsonSerializerSettings()
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Culture = DefaultCulture
            };
            settings.Converters.Add(new DateTimeJsonConverter());
            settings.Converters.Add(new InlineJsonConverter());
         //   settings.Converters.Add(new GuidJsonConverter());
            settings.Converters.Add(new EnumJsonConverter());
            return settings;
        }

    }
}
