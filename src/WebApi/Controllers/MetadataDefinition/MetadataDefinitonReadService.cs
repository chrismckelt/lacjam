using System;
using System.Collections.Generic;
using System.Linq;
using Lacjam.Core.Domain.MetadataDefinitions;
using Lacjam.Core.Domain.MetadataDefinitions.Events;
using Lacjam.Framework.FP;
using Lacjam.Framework.Projection;

namespace Lacjam.WebApi.Controllers.MetadataDefinition
{
    public class MetadataDefinitonReadService : IMetadataDefinitionReadService
    {
        public MetadataDefinitonReadService(IReadStoreRepository<MetadataDefinitionProjection> metaDataDefinitionProjectionRepository,
            IReadStoreRepository<MetadataDefinitionValueProjection> metaDataDefinitionValueProjectionRepository)
        {
            _metaDataDefinitionProjectionRepository = metaDataDefinitionProjectionRepository;
            _metaDataDefinitionValueProjectionRepository = metaDataDefinitionValueProjectionRepository;
        }

        public IMaybe<MetadataDefinitionResource> FindByIdentity(Guid identity)
        {
            return from header in _metaDataDefinitionProjectionRepository.Get(identity)
                   select new MetadataDefinitionResource
                   {
                       Identity = identity,
                       Name = header.Name,
                       Description = header.Description,
                       DataType = header.DataType,
                       Regex = header.Regex,
                       Values = new HashSet<string>(from v in _metaDataDefinitionValueProjectionRepository.ToQueryable() 
                                                    where v.DefinitionId == identity
                                                    select v.Value)
                   };
        }

        public IMaybe<MetadataDefinitionResource> FindByName(string name)
        {
            var result = _metaDataDefinitionProjectionRepository.ToQueryable().FirstOrDefault(x => x.Name == name);
            if (result == null) 
                return new None<MetadataDefinitionResource>();

            return result.ToMetadataDefinitionResource().ToMaybe();
        }

        public IEnumerable<MetadataDefinitionSelectResource> SearchSelections(string q, int pageSize, int pageIndex)
        {
            var query = _metaDataDefinitionProjectionRepository.ToQueryable();
            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(x => x.Name.Contains(q));
            }
            return query.Skip((pageIndex-1)*pageSize).Take(pageSize)
                .Select(x => x.ToMetadataDefinitionSelectResource()).ToArray();
        }

        private readonly IReadStoreRepository<MetadataDefinitionProjection> _metaDataDefinitionProjectionRepository;
        private readonly IReadStoreRepository<MetadataDefinitionValueProjection> _metaDataDefinitionValueProjectionRepository;
    }
}