using System;
using System.Data.Entity.ModelConfiguration;


namespace Lacjam.Framework.Handlers
{
    public class EventHandlerErrorMap : EntityTypeConfiguration<EventHandlerError>
    {
        public EventHandlerErrorMap()
        {
            //Id(x => x.Id, "EventHandlerErrorId");
            //Map(x => x.CreatedUtcDate);
            //Map(x => x.EventType);
            //Map(x => x.Exception).Length(Int32.MaxValue);
            //Map(x => x.Message).Length(Int32.MaxValue);
            //Map(x => x.StackTrace).Length(Int32.MaxValue);
            //Map(x => x.Seq);
            //Map(x => x.Count);

        }
    }
}