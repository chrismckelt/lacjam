using System;
using Lacjam.Core.Domain.MetadataDefinitions;
using Lacjam.Framework.FP;
using Lacjam.Framework.Projection;

namespace Lacjam.WebApi.Controllers.MetadataDefinition
{
    public class MetadataDefinitonReadService : IMetadataDefinitionReadService
    {
        public MetadataDefinitonReadService(IReadStoreRepository<MetadataDefinitionProjection> metaDataDefinitionProjectionRepository,
            IReadStoreRepository<MetadataDefinitionProjection> metaDataDefinitionGroupBagProjectionRepository)
        {
            _metaDataDefinitionProjectionRepository = metaDataDefinitionProjectionRepository;
            _metaDataDefinitionGroupBagProjectionRepository = metaDataDefinitionGroupBagProjectionRepository;
        }

        public IMaybe<MetadataDefinitionResource> FindByIdentity(Guid identity)
        {
            return from header in _metaDataDefinitionProjectionRepository.Get(identity)
                select new MetadataDefinitionResource
                {
                    Identity = identity,
                    Name = header.Name,
                    DataType = header.DataType,
                    Regex = header.Regex,
                    /*Values = (from defns in _metaDataDefinitionGroupBagProjectionRepository.ToQueryable()
                                 where defns.AggregateIdentity == identity
                                 select defns.DefinitionId).ToList()*/
                };
        }

        private readonly IReadStoreRepository<MetadataDefinitionProjection> _metaDataDefinitionProjectionRepository;
        private readonly IReadStoreRepository<MetadataDefinitionProjection> _metaDataDefinitionGroupBagProjectionRepository;
    }
}