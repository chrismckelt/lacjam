using System;
using TypeLite;

namespace Lacjam.WebApi.Controllers.Entities
{
    [TsClass]
    public class EntityListResource
    {
        public int TotalItems { get; set; }
        public Item[] Items { get; set; }

        [TsClass]
        public class Item
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Group { get; set; }
        }
    }
}