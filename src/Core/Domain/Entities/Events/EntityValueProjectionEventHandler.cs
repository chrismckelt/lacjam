using Lacjam.Framework.Events;
using Lacjam.Framework.Extensions;
using Lacjam.Framework.FP;
using Lacjam.Framework.Handlers;
using Lacjam.Framework.Projection;
using System.Linq;

namespace Lacjam.Core.Domain.Entities.Events
{
    public class EntityValueProjectionEventHandler :
        IEventHandler<EntityValueProvidedEvent>,
        IEventHandler<EntityDeletedEvent>,
        IEventHandler<EntityMetadataRemovedEvent>,
        IEventHandler<EntityMetadataDefinitionRemovedEvent>,
        IEventHandler<UpdatedEntityMetadataDefinitionValueEvent>
    {
        public EntityValueProjectionEventHandler(IReadStoreRepository<EntityValueProjection> readStore)
        {
            _readStore = readStore;
        }

        [ImmediateDispatch]
        public void Handle(EntityValueProvidedEvent @event)
        {
            @event.Value.GetStream().Each(value => 
                _readStore.Save(new EntityValueProjection(@event.AggregateIdentity, @event.MetadataDefinitionIdentity, @event.Name.Name, @event.DataType.Tag, @event.Regex, value).ToMaybe()));
        }


        [ImmediateDispatch]
        public void Handle(EntityDeletedEvent @event)
        {
            _readStore.ToQueryable()
                .Where(x => x.EntityIdentity == @event.AggregateIdentity)
                .ToList()
                .Each(x => _readStore.Remove(x.ToMaybe()));
        }

        [ImmediateDispatch]
        public void Handle(EntityMetadataRemovedEvent @event)
        {
            _readStore.ToQueryable()
                .Where(x => x.EntityIdentity == @event.AggregateIdentity)
                .ToList()
                .Each(x => _readStore.Remove(x.ToMaybe()));
        }

        [ImmediateDispatch]
        public void Handle(EntityMetadataDefinitionRemovedEvent @event)
        {
            _readStore.ToQueryable()
                .Where(x => x.MetadataDefinitionIdentity == @event.DefinitionId)
                .Where(x => x.EntityIdentity == @event.AggregateIdentity)
                .ToList()
                .Each(x => _readStore.Remove(x.ToMaybe()));
        }

        [ImmediateDispatch]
        public void Handle(UpdatedEntityMetadataDefinitionValueEvent @event)
        {
            var currentValues = _readStore.ToQueryable()
                                          .Where(x => x.EntityIdentity == @event.AggregateIdentity)
                                          .Where(x => x.MetadataDefinitionIdentity == @event.DefinitionId)
                                          .ToList();

            currentValues.Each(x => _readStore.Remove(x.ToMaybe())); // clear current values

            @event.Supplied.GetStream().Each(value => _readStore.Save(new EntityValueProjection(@event.AggregateIdentity, @event.DefinitionId, @event.Name.Name, @event.DataType.Tag, @event.Regex, value).ToMaybe()));
        }

        private readonly IReadStoreRepository<EntityValueProjection> _readStore;
        
    }
}