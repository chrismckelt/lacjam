using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lacjam.Worker.Jobs
{
    public class AuditJob : JobBase
    {
        public Core.Domain.Audit Audit { get; set; }
    }
}
