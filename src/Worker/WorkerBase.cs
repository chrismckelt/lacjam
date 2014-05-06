using System;

namespace Lacjam.Worker
{
    public abstract class WorkerBase
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        
    }
}
