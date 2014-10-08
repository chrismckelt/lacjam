using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using Lacjam.Core.Domain.MetadataDefinitions;
using Lacjam.Core.Infrastructure;

namespace Lacjam.Core.Domain.MetadataDefinitionGroups
{
    public class MetadataDefinitionGroupProjection 
    {
        public MetadataDefinitionGroupProjection()
        {
            
        }

        public virtual MetadataDefinitionGroupProjection WithNewDescription(MetadataDefinitionGroupDescription description)
        {
            SetUpdated();
            return new MetadataDefinitionGroupProjection
            {
                Description = description.Description,
                Identity = Identity,
                Name = Name,
                Tracking = Tracking
            };
        }

        public virtual MetadataDefinitionGroupProjection WithNewName(MetadataDefinitionGroupName name)
        {
            SetUpdated();
            return new MetadataDefinitionGroupProjection
            {
                Description = Description,
                Identity = Identity,
                Name = name.Name,
                Tracking = Tracking
            };
        }

        public virtual MetadataDefinitionGroupProjection WithTracking(Expression<Func<TrackingBase, Func<TrackingBase>>> tb)
        {
            SetUpdated();
            return new MetadataDefinitionGroupProjection
            {
                Description = Description,
                Identity = Identity,
                Name = Name,
                Tracking = tb.Compile().Invoke(Tracking).Invoke()
            };
        }


        private void SetUpdated()
        {
            if (Tracking != null)
            {
                Tracking.LastModifiedUtcDate = DateTime.UtcNow;
            }
        }

        public virtual Guid Identity{ get; set; }
        public virtual string Name{ get; set; }
        public virtual string Description { get; set; }
        public virtual TrackingBase Tracking { get; set; }
    }
}
