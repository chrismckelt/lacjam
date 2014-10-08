using System;

namespace Lacjam.Core.Domain.MetadataDefinitions
{
    public class MetadataDefinitionSearchResults
    {
        public Hit[] Hits { get; set; }
        public int TotalHits { get; set; }
        
        public class Hit
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string DataType { get; set; }
        }
    }
}