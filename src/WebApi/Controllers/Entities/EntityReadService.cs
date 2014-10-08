using Lacjam.Core.Domain.Entities;
using Lacjam.Core.Domain.MetadataDefinitionGroups;
using Lacjam.Framework.FP;
using Lacjam.Framework.Projection;
using System;
using System.Collections.Generic;
using System.Linq;
using Lacjam.WebApi.Controllers.MetadataDefinitionGroup;

namespace Lacjam.WebApi.Controllers.Entities
{
    public class EntityReadService : IEntityReadService
    {
        public EntityReadService(
            IReadStoreRepository<EntityProjection> entityProjectionRepository, 
            IReadStoreRepository<EntityValueProjection> entityValueProjectionRepository,
            IReadStoreRepository<MetadataDefinitionGroupProjection> groupRepository)
        {
            _entityProjectionRepository = entityProjectionRepository;
            _entityValueProjectionRepository = entityValueProjectionRepository;
            _groupRepository = groupRepository;
        }

        public IMaybe<EntityResource> FindByIdentity(Guid identity)
        {
            return from header in _entityProjectionRepository.Get(identity)
                   let defGroup = _groupRepository.Get(header.MetadataDefinitionGroupIdentity).Value
                   select new EntityResource
                   {
                       Identity = identity,
                       Name = header.Name,
                       DefinitionGroup = defGroup.ToSelectResource(),
                       DefinitionValues = new HashSet<EntityMetadataDefintionResource>(PopulateDefinitionValues(identity))
                   };
        }

        private IEnumerable<EntityMetadataDefintionResource> PopulateDefinitionValues(Guid identity)
        {
            var results = (from v in _entityValueProjectionRepository.ToQueryable()
                           where v.EntityIdentity == identity
                           select v).ToList();

             return (from v in results
                     group v by new {v.Name, v.DataType, v.Regex, v.MetadataDefinitionIdentity} into z
                     select new EntityMetadataDefintionResource
                     {
                        Name = z.Key.Name,
                        DataType =  z.Key.DataType,
                        Regex = z.Key.Regex,
                        MetadataDefinitionIdentity = z.Key.MetadataDefinitionIdentity,
                        Values = new HashSet<string>(z.Select(x => x.Value))
                     });
        }

        public IMaybe<EntityResource> FindByName(string name)
        {
            var result = _entityProjectionRepository.ToQueryable()
                                                    .FirstOrDefault(x => x.Name == name);
            if (result == null)
                return new None<EntityResource>();

            return result.ToEntityResource().ToMaybe();
        }

        private readonly IReadStoreRepository<EntityProjection> _entityProjectionRepository;
        private readonly IReadStoreRepository<EntityValueProjection> _entityValueProjectionRepository;
        private readonly IReadStoreRepository<MetadataDefinitionGroupProjection> _groupRepository;
    }
}