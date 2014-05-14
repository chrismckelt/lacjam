using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lacjam.Worker.Jobs;
using LinqToTwitter;

namespace Lacjam.Worker.Batches
{
    public class Batch : WorkerBase
    {
      

        public virtual IEnumerable<JobBase> Jobs { get; protected set; }

        public void AddJob(JobBase job)
        {
            job.Batch = this;
            if (this.Jobs == null) this.Jobs = new List<JobBase>();

            this.Jobs.ToList().Add(job);
        }
    }
}
