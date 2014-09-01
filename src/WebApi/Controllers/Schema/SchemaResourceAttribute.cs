using System;

namespace Lacjam.WebApi.Controllers.Schema
{
    public class SchemaResourceAttribute
    {
        public Guid Identity { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}