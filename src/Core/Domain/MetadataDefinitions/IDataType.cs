using Newtonsoft.Json;

namespace Lacjam.Core.Domain.MetadataDefinitions
{
    [JsonConverter(typeof(DataTypeConverter))]
    public interface IDataType
    {
        string Tag { get; }
    }
}