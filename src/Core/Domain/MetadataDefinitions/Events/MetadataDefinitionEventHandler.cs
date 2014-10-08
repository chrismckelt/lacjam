using Lacjam.Framework.Events;
using Lacjam.Framework.Exceptions;
using Lacjam.Framework.FP;
using Lacjam.Framework.Handlers;
using Lacjam.Framework.Projection;

namespace Lacjam.Core.Domain.MetadataDefinitions.Events
{
    public class MetadataDefinitionEventHandler : 
        IEventHandler<MetadataDefinitionCreatedEvent>,
        IEventHandler<MetadataDefinitionDeletedEvent>,
        IEventHandler<MetadataDefinitionRegexChangedEvent>,
        IEventHandler<MetadataDefinitionDataTypeChangedEvent>
    {
       
        public MetadataDefinitionEventHandler(IReadStoreRepository<MetadataDefinitionProjection> repository)
        {
            _repository = repository;
        }

        [ImmediateDispatch]
        public void Handle(MetadataDefinitionCreatedEvent @event)
        {
            if (_repository.Exists(@event.AggregateIdentity))
                throw new DuplicateReadModelException();

            var projection = new MetadataDefinitionProjection
            {
                Identity = @event.AggregateIdentity,
                Name = @event.Name.Name,
                Description = @event.Description.Description,
                DataType = @event.DataType,
                Regex = @event.Regex
            };

            _repository.Update(projection.ToMaybe());
        }

        [ImmediateDispatch]
        public void Handle(MetadataDefinitionDeletedEvent @event)
        {
            if (!_repository.Exists(@event.AggregateIdentity))
                throw new AggregateNotFoundException();

            var projection = _repository.Get(@event.AggregateIdentity);
            projection.Foreach(x => x.Delete());

            _repository.Update(projection);
        }

        [ImmediateDispatch]
        public void Handle(MetadataDefinitionRegexChangedEvent @event)
        {
            if (!_repository.Exists(@event.AggregateIdentity))
                throw new AggregateNotFoundException();

            _repository.Update(from p in _repository.Get(@event.AggregateIdentity)
                               select p.WithNewRegularExpression(@event.Regex));
        }

        [ImmediateDispatch]
        public void Handle(MetadataDefinitionDataTypeChangedEvent @event)
        {
            if (!_repository.Exists(@event.AggregateIdentity))
                throw new AggregateNotFoundException();

            _repository.Update(from p in _repository.Get(@event.AggregateIdentity)
                               select p.WithDataType(@event.DataType));
        }

        private readonly IReadStoreRepository<MetadataDefinitionProjection> _repository;
    }
}