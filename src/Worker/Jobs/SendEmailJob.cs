using Lacjam.Core.Domain;

namespace Lacjam.Worker.Jobs
{
    public class SendEmailJob : JobBase
    {
        public Email Email { get; set; }
    }
}