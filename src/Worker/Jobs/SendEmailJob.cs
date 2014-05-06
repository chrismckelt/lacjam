using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lacjam.Core.Domain;

namespace Lacjam.Worker.Jobs
{
    public class SendEmailJob : JobBase
    {
        public Email Email { get; set; }
    }
}
