
using Lacjam.Core.Infrastructure;
using Lacjam.Framework.Exceptions;
using Lacjam.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Lacjam.WebApi.Controllers.MetadataDefinition;
using TypeLite;

namespace Lacjam.WebApi.Controllers.MetadataDefinitionGroup
{
    [TsClass]
    public class MetadataDefinitionGroupResource 
    {
        public MetadataDefinitionGroupResource()
        {
            Definitions = new MetadataDefinitionSelectResource[0];
        }

        public bool DiscriptionMatches(MetadataDefinitionGroupResource other)
        {
            if(other == null)
                throw new InvariantGuardFailureException();

            return other.PublicInstancePropertiesEqual(this);
        }

        public bool DefinitionIdsMatch(MetadataDefinitionGroupResource other)
        {
            if (other == null)
                throw new InvariantGuardFailureException();

            if (Definitions.Count() != other.Definitions.Count())
                return false;

            var result = Definitions.Intersect(other.Definitions).Count() == Definitions.Count();
            return result;
        }

        public void Clean()
        {
            Name = Name.Trim();

            if (!String.IsNullOrEmpty(Description))
                Description = Description.Trim();
        }
        
        public Guid Identity { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters ")]
        public string Name { get; set; }
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters ")]
        public string Description { get; set; }
        public TrackingBase Tracking { get; set; }
        public MetadataDefinitionSelectResource[] Definitions { get; set; }
    }
}