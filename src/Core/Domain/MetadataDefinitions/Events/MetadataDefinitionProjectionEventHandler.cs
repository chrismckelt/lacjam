using Lacjam.Framework.Events;
using Lacjam.Framework.FP;
using Lacjam.Framework.Handlers;
using Lacjam.Framework.Projection;

namespace Lacjam.Core.Domain.MetadataDefinitions.Events
{

    public class MetadataDefinitionProjectionEventHandler : 
        IEventHandler<MetadataDefinitionCreatedEvent>,
        IEventHandler<MetadataDefinitionRegexChangedEvent>,
        IEventHandler<MetadataDefinitionDeletedEvent>,
        IEventHandler<ReLabelMetadataDefinitionEvent>
    {

        public MetadataDefinitionProjectionEventHandler(IReadStoreRepository<MetadataDefinitionProjection> repository)
        {
            _repository = repository;
        }

        [ImmediateDispatch]
        public void Handle(MetadataDefinitionCreatedEvent @event)
        {
             var projection = new MetadataDefinitionProjection(@event.AggregateIdentity, @event.Name.Name, @event.DataType.Tag, @event.Regex);
            _repository.Save(projection.ToMaybe());
        }

        [ImmediateDispatch]
        public void Handle(MetadataDefinitionRegexChangedEvent @event)
        {
            _repository.Update(from projection in _repository.Get(@event.AggregateIdentity)
                               select projection.WithNewRegularExpression(@event.Regex));
        }

        [ImmediateDispatch]
        public void Handle(MetadataDefinitionDeletedEvent @event)
        {
            _repository.Remove(from projection in _repository.Get(@event.AggregateIdentity) select projection);
        }

        [ImmediateDispatch]
        public void Handle(ReLabelMetadataDefinitionEvent @event)
        {
           _repository.Update( from projection in _repository.Get(@event.AggregateIdentity)
                               select projection.WithNewDetails(@event.Name));

        }

        private readonly IReadStoreRepository<MetadataDefinitionProjection> _repository;
        
    }
}
