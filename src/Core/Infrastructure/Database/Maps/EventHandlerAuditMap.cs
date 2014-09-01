using System;
using FluentNHibernate.Mapping;
using Lacjam.Core.Infrastructure.Database.Conventions;
using Lacjam.Framework.Handlers;

namespace Lacjam.Core.Infrastructure.Database.Maps
{
    public class EventHandlerAuditMap : ClassMap<EventHandlerAudit>
    {
        public EventHandlerAuditMap()
        {
            Table("EventAudit");
            Id(x => x.Id).GeneratedBy.Guid(); ;
            Map(x => x.CreatedUtcDate).Default("GETUTCDATE()"); ;
            Map(x => x.Seq);
            Map(x => x.EventId);
            Map(x => x.EventProcessedUtcDate);

            Map(x => x.Result).CustomSqlType(new EnumMapper<EventHandlerAuditResult>().SqlType.ToString());

            Map(x => x.Message).Length(Int32.MaxValue);
        }
    }
}