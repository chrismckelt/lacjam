using System;
using Lacjam.Core.Infrastructure;

namespace Lacjam.Core.Domain.Entities
{
    public class EntityValueProjection : IEquatable<EntityValueProjection>
    {

        protected EntityValueProjection()
        {
            Identity = Guid.NewGuid();
            Tracking = new TrackingBase();
        }

        public EntityValueProjection(Guid entityIdentity, Guid metadataDefinitionIdentity, string name, string dataType, string regex, string value) : this()
        {
            EntityIdentity = entityIdentity;
            MetadataDefinitionIdentity = metadataDefinitionIdentity;
            Name = name;
            DataType = dataType;
            Regex = regex;
            Value = value;
        }

        public virtual bool Equals(EntityValueProjection other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Identity.Equals(other.Identity);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EntityValueProjection) obj);
        }

        public override int GetHashCode()
        {
            return Identity.GetHashCode();
        }

        public static bool operator ==(EntityValueProjection left, EntityValueProjection right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EntityValueProjection left, EntityValueProjection right)
        {
            return !Equals(left, right);
        }

        public virtual Guid Identity { get; set; }
        public virtual Guid EntityIdentity { get; set; }
        public virtual Guid MetadataDefinitionIdentity { get; set; }
        public virtual string Regex { get; set; }
        public virtual string DataType { get; set; }
        public virtual string Name { get; set; }
        public virtual string Value { get; set; }
        public virtual TrackingBase Tracking { get; set; }
    }
}