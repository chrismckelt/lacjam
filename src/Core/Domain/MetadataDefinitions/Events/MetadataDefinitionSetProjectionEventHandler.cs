using System.Linq;
using Lacjam.Framework.Events;
using Lacjam.Framework.FP;
using Lacjam.Framework.Handlers;
using Lacjam.Framework.Projection;

namespace Lacjam.Core.Domain.MetadataDefinitions.Events
{
    public class MetadataDefinitionSetProjectionEventHandler : 
        IEventHandler<MetadataDefinitionAddAllowableValueEvent>,
        IEventHandler<MetadataDefinitionDeletedEvent>
    {
        public MetadataDefinitionSetProjectionEventHandler(IReadStoreRepository<MetadataDefinitionSetProjection> repository)
        {
            _repository = repository;
        }

        [ImmediateDispatch]
        public void Handle(MetadataDefinitionAddAllowableValueEvent @event)
        {
            var projection = new MetadataDefinitionSetProjection(@event.AggregateIdentity, @event.Value.GetValue());
            _repository.Save(projection.ToMaybe());
        }

        [ImmediateDispatch]
        public void Handle(MetadataDefinitionDeletedEvent @event)
        {
            var projections = (from projection in _repository.ToQueryable().Where(x => x.DefinitionId == @event.AggregateIdentity)
                select projection)
                .ToList();

            foreach (var projection in projections)
                _repository.Remove(projection.ToMaybe());
        }

        private readonly IReadStoreRepository<MetadataDefinitionSetProjection> _repository;
    }
}