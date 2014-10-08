using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Lacjam.WebApi.Controllers.MetadataDefinitionGroup;
using TypeLite;

namespace Lacjam.WebApi.Controllers.Entities
{
    [TsClass]
    public class EntityResource
    {
        public EntityResource()
        {
            DefinitionValues = new HashSet<EntityMetadataDefintionResource>();
        }

        public bool DiscriptionMatches(EntityResource other)
        {
            if (other == null)
                return false;

            return Equals(Name, other.Name);
        }


        public bool ValuesMatch(EntityResource other)
        {
            if (other == null)
                return false;

            if (DefinitionValues == null)
                return other.DefinitionValues == null || !other.DefinitionValues.Any();

            if (other.DefinitionValues == null)
                return DefinitionValues == null || !DefinitionValues.Any();

            return DefinitionValues.Count == other.DefinitionValues.Count && DefinitionValues.Intersect(other.DefinitionValues).Count() == DefinitionValues.Count;
        }

        [Required]
        public Guid Identity { get; set; }

        [Required]
        public MetadataDefinitionGroupSelectResource DefinitionGroup { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string Name { get; set; }

        [Required]
        public HashSet<EntityMetadataDefintionResource> DefinitionValues { get; set; }
    }
}