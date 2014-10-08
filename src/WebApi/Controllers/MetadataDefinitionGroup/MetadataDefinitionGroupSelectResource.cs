    using System;
    using Lacjam.WebApi.Controllers.MetadataDefinition;

namespace Lacjam.WebApi.Controllers.MetadataDefinitionGroup
{
    public class MetadataDefinitionGroupSelectResource
    {
        public Guid Id { get; set; }
        public string Text { get; set; }

        protected bool Equals(MetadataDefinitionSelectResource other)
        {
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MetadataDefinitionSelectResource) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}