using System;
using System.Collections.Generic;
using TypeLite;

namespace Lacjam.WebApi.Controllers.MetadataDefinition
{
    [TsClass]
    public class MetadataDefinitionResource
    {

        public bool DiscriptionMatches(MetadataDefinitionResource other)
        {
            if (other == null)
                return false;

            return Equals(Name, other.Name);
        }

        public bool RegexMatches(MetadataDefinitionResource other)
        {
            if (other == null)
                return false;

            return Equals(Regex, other.Regex);
        }

        public Guid Identity { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public string Regex { get; set; }
        public List<string> Values { get; set; }

        
    }
}