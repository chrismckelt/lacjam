using System;
using System.Linq;
using NHibernate.Linq;
using Lacjam.Core.Domain.MetadataDefinitionGroups;
using Lacjam.Core.Domain.MetadataDefinitionGroups.Events;
using Lacjam.Core.Domain.MetadataDefinitions;
using Lacjam.Core.Services;
using Lacjam.Framework.Events;
using Lacjam.Framework.Handlers;
using Lacjam.Framework.Projection;

namespace Lacjam.Core.Domain.Entities.Events
{
    public class EntityIndexEventHandler : 
        IEventHandler<EntityCreatedEvent>,
        IEventHandler<EntityDeletedEvent>,
        IEventHandler<EntityRenamedEvent>,
        IEventHandler<EntityChangedGroupEvent>,
        IEventHandler<EntityValueProvidedEvent>,
        IEventHandler<EntityMetadataRemovedEvent>,
        IEventHandler<EntityMetadataDefinitionRemovedEvent>,
        IEventHandler<MetadataDefinitionGroupNameChangedEvent>
    {
        private readonly IEntityIndexer _indexer;
        private readonly IReadStoreRepository<EntityProjection> _entityRepository;
        private readonly IReadStoreRepository<EntityValueProjection> _entityValueRepository;
        private readonly IReadStoreRepository<MetadataDefinitionGroupProjection> _groupRepository;

        public EntityIndexEventHandler(
            IEntityIndexer indexer, 
            IReadStoreRepository<EntityProjection> entityRepository,
            IReadStoreRepository<EntityValueProjection> entityValueRepository,
            IReadStoreRepository<MetadataDefinitionGroupProjection> groupRepository)
        {
            _indexer = indexer;
            _entityRepository = entityRepository;
            _entityValueRepository = entityValueRepository;
            _groupRepository = groupRepository;
        }

        public void Handle(EntityCreatedEvent @event)
        {
            UpdateIndex(@event.AggregateIdentity);
        }

        public void Handle(EntityDeletedEvent @event)
        {
            _indexer.DeleteIndex(@event.AggregateIdentity);
        }

        public void Handle(EntityRenamedEvent @event)
        {
            UpdateIndex(@event.AggregateIdentity);
        }

        public void Handle(EntityValueProvidedEvent @event)
        {
            UpdateIndex(@event.AggregateIdentity);
        }

        public void Handle(EntityMetadataRemovedEvent @event)
        {
            UpdateIndex(@event.AggregateIdentity);
        }

        public void Handle(EntityMetadataDefinitionRemovedEvent @event)
        {
            UpdateIndex(@event.AggregateIdentity);
        }

        public void Handle(EntityChangedGroupEvent @event)
        {
            UpdateIndex(@event.AggregateIdentity);
        }

        public void Handle(MetadataDefinitionGroupNameChangedEvent @event)
        {
            var group = _groupRepository.ToQueryable().FirstOrDefault(x => x.Identity == @event.AggregateIdentity);

            _indexer.RenameGroup(group);
        }

        private void UpdateIndex(Guid entityId)
        {
            var entities = _entityRepository.ToQueryable().Where(x => x.Identity == entityId).ToFuture();
            var values = _entityValueRepository.ToQueryable().Where(x => x.EntityIdentity == entityId).ToFuture();
            
            foreach (var entity in entities)
            {
                var group = _groupRepository.ToQueryable().FirstOrDefault(x => x.Identity == entity.MetadataDefinitionGroupIdentity);
                _indexer.SaveIndex(entity, group, values);
            }
        }
    }
}