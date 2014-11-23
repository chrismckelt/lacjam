using System;
using System.Collections.Generic;

namespace Lacjam.Core.Domain.Entities
{
    public class EntitySearchResults
    {
        public Hit[] Hits { get; set; }
        public int TotalHits { get; set; }
        
        public class Hit
        {
            public Guid Id { get; set; }
            public string Text { get; set; }
            public string Group { get; set; }

            public string Description { get; set; }
            public string[] Tags { get; set; }
        }
    }
}