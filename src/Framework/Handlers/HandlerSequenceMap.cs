

using System.Data.Entity.ModelConfiguration;

namespace Lacjam.Framework.Handlers
{
    public class HandlerSequenceMap : EntityTypeConfiguration<HandlerSequence>
    {
        public HandlerSequenceMap()
        {
            //Id(x => x.Name, "Dispatcher");
           // Map(x => x.Seq).Column("Pointer");
        }
    }
}