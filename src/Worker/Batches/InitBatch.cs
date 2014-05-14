using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lacjam.Worker.Jobs;

namespace Lacjam.Worker.Batches
{
    public class InitBatch : IContainBatches
    {
        public InitBatch()
        {
            this.Batches = new List<Batch>();
            this.Batches.Add(new SwellNetBatch());
        }

        public IList<Batch> Batches { get; private set; }

        public class SwellNetBatch : Batch
        {
            public SwellNetBatch()
            {
                this.AddJob(new PrintBatchJob());
            }
        }
    }
}
