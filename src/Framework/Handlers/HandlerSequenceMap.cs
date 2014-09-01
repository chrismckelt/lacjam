using FluentNHibernate.Mapping;

namespace Lacjam.Framework.Handlers
{
    public class HandlerSequenceMap : ClassMap<HandlerSequence>
    {
        public HandlerSequenceMap()
        {
            Id(x => x.Name, "Dispatcher");
            Map(x => x.Seq).Column("Pointer");
        }
    }
}