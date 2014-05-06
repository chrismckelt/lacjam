using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lacjam.Worker.Batches
{
    public interface IContainBatches
    {
        IEnumerable<IContainBatches> Batches { get; } 
    }
}
