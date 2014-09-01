using System;

namespace Lacjam.Framework.Model
{
    public abstract class Entity : IEntity, IEquatable<Entity>
    {
       
        protected Entity()
        {
            Identity = Guid.Empty;
        }

        protected Entity(Guid identity)
        {
            Identity = identity;
        }

        public Guid GetIdentity()
        {
            return Identity;
        }

        public bool Equals(Entity other)
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
            return Equals((Entity)obj);
        }

        public override int GetHashCode()
        {
            return Identity.GetHashCode();
        }

        public static bool operator ==(Entity left, Entity right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !Equals(left, right);
        }

        protected Guid Identity { get; set; }
        
    }
}