using System.Collections.Generic;
using Newtonsoft.Json;
using Lacjam.Core.Domain.MetadataDefinitions;

namespace Lacjam.Core.Domain.Entities
{
    [JsonConverter(typeof(EntityValueConverter))]
    public interface IValue
    {
        void Validate(IDataType dataType, string regex);
        IEnumerable<string> GetStream();
    }
}