namespace Lacjam.Worker.Jobs
{
    public class AuditJob : JobBase
    {
        public Core.Domain.Audit Audit { get; set; }
    }
}