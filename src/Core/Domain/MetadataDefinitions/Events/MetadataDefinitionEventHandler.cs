using Lacjam.Framework.Exceptions;
using Lacjam.Framework.FP;
using Lacjam.Framework.Handlers;
using Lacjam.Framework.Projection;

namespace Lacjam.Core.Domain.MetadataDefinitions.Events
{
    public class MetadataDefinitionEventHandler : IEventHandler<MetadataDefinitionCreatedEvent>
    {
        private readonly IReadStoreRepository<MetadataDefinitionProjection> _repository;

        public MetadataDefinitionEventHandler(IReadStoreRepository<MetadataDefinitionProjection> repository)
        {
            _repository = repository;
        }

        public void Handle(MetadataDefinitionCreatedEvent @event)
        {
            if (_repository.Exists(@event.AggregateIdentity))
                throw new DuplicateReadModelException();

            var readmodel = new MetadataDefinitionProjection
            {
                Identity = @event.AggregateIdentity,
                Name = @event.Name.Name
            };

            _repository.Update(readmodel.ToMaybe());
        }
    }
}