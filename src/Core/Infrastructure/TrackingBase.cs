using System;

namespace Lacjam.Core.Infrastructure
{
    public class TrackingBase 
    {
        public TrackingBase()
        {
            this.CreatedUtcDate = DateTime.UtcNow;
            this.IsDeleted = false;
            this.IsActive = false;
        }

       public virtual bool IsActive { get;  set; }
       public virtual bool IsDeleted { get; set; }
       public virtual DateTime CreatedUtcDate { get; protected set; }
       public virtual DateTime? LastModifiedUtcDate { get; set; }

    }
}
