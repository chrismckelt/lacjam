using System;
using System.Collections.Generic;
using System.Linq;
using Lacjam.Core.Infrastructure;
using Lacjam.Framework.Exceptions;
using Lacjam.Framework.Extensions;
using TypeLite;

namespace Lacjam.WebApi.Controllers.MetadataDefinitionGroup
{
    [TsClass]
    public class MetadataDefinitionGroupResource : TrackingBase
    {

        public MetadataDefinitionGroupResource()
        {
            SelectedDefinitionIds = new List<Guid>();
        }

        public bool DiscriptionMatches(MetadataDefinitionGroupResource other)
        {
            if(other == null)
                throw new InvariantGuardFailureException();

            return other.PublicInstancePropertiesEqual<MetadataDefinitionGroupResource>(this);
        }

        public bool DefinitionIdsMatch(MetadataDefinitionGroupResource other)
        {
            if (other == null)
                throw new InvariantGuardFailureException();

            return SelectedDefinitionIds.Intersect(other.SelectedDefinitionIds).Count() == SelectedDefinitionIds.Count;
        }

        public Guid Identity { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<Guid> SelectedDefinitionIds { get; set; }
    }
}