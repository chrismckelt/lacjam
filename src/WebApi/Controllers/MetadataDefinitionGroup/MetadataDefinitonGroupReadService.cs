using System;
using System.Linq;
using Lacjam.Core.Domain.MetadataDefinitionGroups;
using Lacjam.Framework.FP;
using Lacjam.Framework.Projection;

namespace Lacjam.WebApi.Controllers.MetadataDefinitionGroup
{
    public class MetadataDefinitonGroupReadService : IMetadataDefinitonGroupReadService
    {
        public MetadataDefinitonGroupReadService(IReadStoreRepository<MetadataDefinitionGroupProjection> metaDataDefinitionGroupProjectionRepository, 
                                                 IReadStoreRepository<MetadataDefinitionGroupBagProjection> metaDataDefinitionGroupBagProjectionRepository)
        {
            _metaDataDefinitionGroupProjectionRepository = metaDataDefinitionGroupProjectionRepository;
            _metaDataDefinitionGroupBagProjectionRepository = metaDataDefinitionGroupBagProjectionRepository;
        }

        public IMaybe<MetadataDefinitionGroupResource> FindByIdentity(Guid identity)
        {
            return from header in _metaDataDefinitionGroupProjectionRepository.Get(identity)
                   
                        select new MetadataDefinitionGroupResource
                        {
                            Identity = identity,
                            Name = header.Name,
                            Description = header.Description,
                            SelectedDefinitionIds = (from defns in _metaDataDefinitionGroupBagProjectionRepository.ToQueryable()
                                                     where defns.AggregateIdentity == identity
                                                     select defns.DefinitionId).ToList()
                        };
        }

        private readonly IReadStoreRepository<MetadataDefinitionGroupProjection> _metaDataDefinitionGroupProjectionRepository;
        private readonly IReadStoreRepository<MetadataDefinitionGroupBagProjection> _metaDataDefinitionGroupBagProjectionRepository;
    }
}