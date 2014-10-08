using System;
using Lacjam.Core.Services;
using Lacjam.Framework.Handlers;
using Lacjam.Framework.Projection;

namespace Lacjam.Core.Domain.MetadataDefinitions.Events
{
    public class MetadataDefinitionIndexEventHandler :
        IEventHandler<MetadataDefinitionCreatedEvent>,
        IEventHandler<MetadataDefinitionDeletedEvent>,
        IEventHandler<ReLabelMetadataDefinitionEvent>,
        IEventHandler<ReLabelMetadataDefinitionDescriptionEvent>
    {
        private readonly IReadStoreRepository<MetadataDefinitionProjection> _repository;
        private readonly IMetadataDefinitionIndexer _indexer;

        public MetadataDefinitionIndexEventHandler(IReadStoreRepository<MetadataDefinitionProjection> repository, IMetadataDefinitionIndexer indexer)
        {
            _repository = repository;
            _indexer = indexer;
        }

        public void Handle(MetadataDefinitionCreatedEvent @event)
        {
            UpdateIndex(@event.AggregateIdentity);
        }

        public void Handle(MetadataDefinitionDeletedEvent @event)
        {
            _indexer.DeleteIndex(@event.AggregateIdentity);
        }

        public void Handle(ReLabelMetadataDefinitionEvent @event)
        {
            UpdateIndex(@event.AggregateIdentity);
        }

        public void Handle(ReLabelMetadataDefinitionDescriptionEvent @event)
        {
            UpdateIndex(@event.AggregateIdentity);
        }

        private void UpdateIndex(Guid aggregateIdentity)
        {
            var definition = _repository.Reference(aggregateIdentity);
            _indexer.SaveIndex(definition);
        }
    }
}