using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TypeLite;

namespace Lacjam.WebApi.Controllers.Entities
{
    [TsClass]
    public class EntityMetadataDefintionResource : IEquatable<EntityMetadataDefintionResource>
    {
        public bool Equals(EntityMetadataDefintionResource other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Values, other.Values) && string.Equals(Regex, other.Regex) && string.Equals(DataType, other.DataType) && string.Equals(Name, other.Name) && MetadataDefinitionIdentity.Equals(other.MetadataDefinitionIdentity);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EntityMetadataDefintionResource) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Values != null ? Values.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Regex != null ? Regex.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (DataType != null ? DataType.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ MetadataDefinitionIdentity.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(EntityMetadataDefintionResource left, EntityMetadataDefintionResource right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EntityMetadataDefintionResource left, EntityMetadataDefintionResource right)
        {
            return !Equals(left, right);
        }

        public EntityMetadataDefintionResource()
        {
            Values = new HashSet<string>();
        }

        [Required]
        public Guid MetadataDefinitionIdentity { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Definition Name cannot exceed 200 characters")]
        public string Name { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Definition DataType cannot exceed 200 characters")]
        public string DataType { get; set; }

        [StringLength(200, ErrorMessage = "Definition Regex cannot exceed 200 characters")]
        public string Regex { get; set; }

        [Required]
        public HashSet<string> Values { get; set; } 
    }
}