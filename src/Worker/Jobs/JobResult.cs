using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lacjam.Worker.Jobs
{
    public class JobResult : JobBase
    {
        public Exception ExceptionCaught { get; protected set; }
        public JobBase JobBase { get; protected set; }
        public bool Success { get; protected set; }
        public string ResultMessage { get; protected set; }
        public bool Resubmit { get; protected set; }

        public JobResult(JobBase jobBase, bool success, string resultMessage, bool resubmit = false, Exception exception = null)
        {
            ExceptionCaught = exception;
            JobBase = jobBase;
            Success = success;
            ResultMessage = resultMessage;
            Resubmit = resubmit;
        }

        public override string ToString()
        {
            return string.Format("{0}, JobBase: {1}, Success: {2}, ResultMessage: {3}, Resubmit: {4}", base.ToString(), JobBase, Success, ResultMessage, Resubmit);
        }
    }
}
