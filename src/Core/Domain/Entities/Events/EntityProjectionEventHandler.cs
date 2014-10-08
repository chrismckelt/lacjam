using Lacjam.Framework.Events;
using Lacjam.Framework.FP;
using Lacjam.Framework.Handlers;
using Lacjam.Framework.Projection;

namespace Lacjam.Core.Domain.Entities.Events
{
    public class EntityProjectionEventHandler : 
        IEventHandler<EntityCreatedEvent>,
        IEventHandler<EntityDeletedEvent>,
        IEventHandler<EntityRenamedEvent>,
        IEventHandler<EntityChangedGroupEvent>
    {
        public EntityProjectionEventHandler(IReadStoreRepository<EntityProjection> readStore)
        {
            _readStore = readStore;
        }

        [ImmediateDispatch]
        public void Handle(EntityCreatedEvent @event)
        {
            var projection = new EntityProjection(@event.AggregateIdentity,@event.MetadataDefinitionGroupIdentity, @event.Name.Name);
            _readStore.Save(projection.ToMaybe());
        }

        [ImmediateDispatch]
        public void Handle(EntityDeletedEvent @event)
        {
            _readStore.Remove(from p in _readStore.Get(@event.AggregateIdentity) select p);
        }

        [ImmediateDispatch]
        public void Handle(EntityRenamedEvent @event)
        {
            _readStore.Update(from p in _readStore.Get(@event.AggregateIdentity) select p.WithName(@event.Name.Name));
        }

        private readonly IReadStoreRepository<EntityProjection> _readStore;


        [ImmediateDispatch]
        public void Handle(EntityChangedGroupEvent @event)
        {
            _readStore.Update(from p in _readStore.Get(@event.AggregateIdentity) select p.WithGroup(@event.DefinitionGroupId));
        }
    }
}