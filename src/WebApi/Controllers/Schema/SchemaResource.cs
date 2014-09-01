using System;
using System.Collections.Generic;

namespace Lacjam.WebApi.Controllers.Schema
{
    public class SchemaResource
    {
        public SchemaResource()
        {
            Attributes = new List<SchemaResourceAttribute>();
        }

        public Guid Identity { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<SchemaResourceAttribute> Attributes { get; set; }
    }
}