using System;
using System.Linq;

using Lacjam.Core.Domain.MetadataDefinitionGroups;
using Lacjam.Framework.FP;
using Lacjam.Framework.Projection;

namespace Lacjam.Core.Services
{
    public class SchemaService : ISchemaService
    {
        public SchemaService(IReadStoreRepository<MetadataDefinitionGroupProjection> conceptRepository, IReadStoreRepository<MetadataDefinitionGroupMetadataProjection> attributeRepository )
        {
            _conceptRepository = conceptRepository;
            _attributeRepository = attributeRepository;
        }

        public IMaybe<SchemaTransferObject> GetSchemaFor(Guid identity)
        {
            var concept = _conceptRepository.Get(identity);

            return  from c in concept
                    from a in _attributeRepository.ToQueryable().Where(x => x.ConceptIdentity == identity).ToMaybe()
                    select new SchemaTransferObject(c, a);
        }

        private readonly IReadStoreRepository<MetadataDefinitionGroupProjection> _conceptRepository;
        private readonly IReadStoreRepository<MetadataDefinitionGroupMetadataProjection> _attributeRepository;
    }
}