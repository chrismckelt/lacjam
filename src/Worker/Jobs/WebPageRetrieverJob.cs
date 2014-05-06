using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lacjam.Worker.Jobs
{
    public class WebPageRetrieverJob : JobBase
    {
        public string Url { get; set; }
    }
}
