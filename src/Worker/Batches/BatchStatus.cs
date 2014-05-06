using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lacjam.Worker.Batches
{
    public enum BatchStatus
    {
        Waiting,
        Processing,
        Success,
        Failed 
    }
}
