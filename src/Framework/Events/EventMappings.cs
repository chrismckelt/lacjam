using FluentNHibernate.Mapping;

namespace Lacjam.Framework.Events
{
    public class EventDescriptorMap : ClassMap<EventDescriptor>
    {
        public EventDescriptorMap()
        {

            Table("Event");
            Id(x => x.EventId).GeneratedBy.GuidComb();
            Map(x => x.Seq).ReadOnly();
           

            Component(@event => @event.Header, header =>
            {
                header.Map(x => x.AggregateId);
                header.Map(x => x.SchemaVersion);
                header.Map(x => x.Author);
                header.Map(x => x.CreatedUtcDate);
                header.Map(x => x.Version);

            });
            Map(x => x.EventData)
                .Columns.Add("EventType")
                .Columns.Add("EventData")         
                .CustomType<EventDataJsonType>();

          //  HasOne<Event>(x => x.EventData.Event);
        }
    }
}