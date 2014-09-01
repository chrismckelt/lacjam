using System.Linq;
using Lacjam.Framework.Events;
using Lacjam.Framework.Extensions;
using Lacjam.Framework.FP;
using Lacjam.Framework.Handlers;
using Lacjam.Framework.Projection;

namespace Lacjam.Core.Domain.MetadataDefinitionGroups.Events
{
    public class MetadataDefinitionGroupBagProjectionEventHandler : 
        IEventHandler<MetadataDefinitionGroupAssociatedMetadataEvent>,
        IEventHandler<MetadataDefinitionGroupAttributesClearedEvent>
    {
        public MetadataDefinitionGroupBagProjectionEventHandler(IReadStoreRepository<MetadataDefinitionGroupBagProjection> repository)
        {
            _repository = repository;
        }

        [ImmediateDispatch]
        public void Handle(MetadataDefinitionGroupAssociatedMetadataEvent @event)
        {
            var readmodel = new MetadataDefinitionGroupBagProjection
            {
                AggregateIdentity = @event.AggregateIdentity,
                DefinitionId = @event.DefinitionId
            };

            _repository.Save(readmodel.ToMaybe());

        }

        [ImmediateDispatch]
        public void Handle(MetadataDefinitionGroupAttributesClearedEvent @event)
        {
            _repository.ToQueryable()
                .Where(x => x.AggregateIdentity == @event.AggregateIdentity)
                .Each(x => _repository.Remove(x.ToMaybe()));
        }

        private readonly IReadStoreRepository<MetadataDefinitionGroupBagProjection> _repository;
        
    }

    
}