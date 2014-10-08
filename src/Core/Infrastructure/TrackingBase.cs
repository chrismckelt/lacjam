using System;

namespace Lacjam.Core.Infrastructure
{
    public class TrackingBase
    {
        public TrackingBase()
        {
            this.CreatedUtcDate = DateTime.UtcNow;
            this.IsDeleted = false;
        }

        public virtual TrackingBase WithUpdatedDateTime()
        {
            return new TrackingBase
            {
                CreatedUtcDate = CreatedUtcDate,
                IsDeleted = IsDeleted,
                LastModifiedUtcDate = DateTime.UtcNow
            };
        }

        public virtual bool IsDeleted { get; set; }
        public virtual DateTime CreatedUtcDate { get; protected set; }
        public virtual DateTime? LastModifiedUtcDate { get; set; }

        
    }
}
