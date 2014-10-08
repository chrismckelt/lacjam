using System;
using System.Linq.Expressions;
using Lacjam.Core.Infrastructure;

namespace Lacjam.Core.Domain.MetadataDefinitions.Events
{
    public class MetadataDefinitionValueProjection
    {

        protected MetadataDefinitionValueProjection(){}

        public MetadataDefinitionValueProjection(Guid definitionId, string value) : base()
        {
            this.Identity = Guid.NewGuid();
            DefinitionId = definitionId;
            Value = value;
            this.Tracking = new TrackingBase();
            SetUpdated();
        }

        public virtual MetadataDefinitionValueProjection WithTracking(Expression<Func<TrackingBase, Func<TrackingBase>>> tb)
        {
            SetUpdated();
            return new MetadataDefinitionValueProjection
            {
                Value = Value,
                Identity = Identity,
                DefinitionId = DefinitionId,
                Tracking = Tracking,
            };
        }

        private void SetUpdated()
        {
            if (Tracking == null){
                Tracking = new TrackingBase();
              
            }

            Tracking.LastModifiedUtcDate = DateTime.UtcNow;
        }

        public virtual Guid Identity { get; set; }
        public virtual Guid DefinitionId { get; set; }
        public virtual string Value { get; set; }
        public virtual TrackingBase Tracking { get; set; }
    }
}