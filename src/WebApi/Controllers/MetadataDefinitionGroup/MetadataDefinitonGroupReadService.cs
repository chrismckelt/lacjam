using System;
using System.Collections.Generic;
using System.Linq;

using Lacjam.Core.Domain.MetadataDefinitionGroups;
using Lacjam.Core.Domain.MetadataDefinitions;
using Lacjam.Core.Domain.MetadataDefinitions.Events;
using Lacjam.Framework.FP;
using Lacjam.Framework.Projection;
using Lacjam.WebApi.Controllers.MetadataDefinition;

namespace Lacjam.WebApi.Controllers.MetadataDefinitionGroup
{
    public class MetadataDefinitonGroupReadService : IMetadataDefinitonGroupReadService
    {
        public MetadataDefinitonGroupReadService(
            IReadStoreRepository<MetadataDefinitionGroupProjection> metaDataDefinitionGroupProjectionRepository, 
            IReadStoreRepository<MetadataDefinitionGroupBagProjection> metaDataDefinitionGroupBagProjectionRepository,
            IReadStoreRepository<MetadataDefinitionProjection> definitionRepository,
            IReadStoreRepository<MetadataDefinitionValueProjection> definitionValueRepository)
        {
            _metaDataDefinitionGroupProjectionRepository = metaDataDefinitionGroupProjectionRepository;
            _metaDataDefinitionGroupBagProjectionRepository = metaDataDefinitionGroupBagProjectionRepository;
            _definitionRepository = definitionRepository;
            _definitionValueRepository = definitionValueRepository;
        }

        public IMaybe<MetadataDefinitionGroupResource> FindByIdentity(Guid identity)
        {
            return from header in _metaDataDefinitionGroupProjectionRepository.Get(identity)
                   
                        select new MetadataDefinitionGroupResource
                        {
                            Identity = identity,
                            Name = header.Name,
                            Description = header.Description,
                            Tracking = header.Tracking,
                            Definitions = (from bag in _metaDataDefinitionGroupBagProjectionRepository.ToQueryable()
                                                     where bag.AggregateIdentity == identity
                                            join def in _definitionRepository.ToQueryable() on bag.DefinitionId equals def.Identity         
                                                     select def.ToMetadataDefinitionSelectResource()).ToArray()
                        };
        }

        public IMaybe<MetadataDefinitionGroupResource> FindByName(string name)
        {
            var result = _metaDataDefinitionGroupProjectionRepository.ToQueryable()
                                                                     .FirstOrDefault(a => a.Name == name);

            return result == null ? MaybeExtensions.ToMaybe<MetadataDefinitionGroupResource>(null) : result.ToMetadataDefinitionGroupResource().ToMaybe();
        }

        public IEnumerable<MetadataDefinitionGroupResource> GetAll()
        {
            var all =  _metaDataDefinitionGroupProjectionRepository.ToQueryable();

            return all.Select(proj => proj.ToMetadataDefinitionGroupResource());
        }

        public IEnumerable<MetadataDefinitionGroupSelectResource> SearchSelections(string q, int pageSize, int pageIndex)
        {
            var query = _metaDataDefinitionGroupProjectionRepository.ToQueryable();
            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(x => x.Name.Contains(q));
            }
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize)
                .Select(x => x.ToSelectResource()).ToArray();
        }

        public IEnumerable<MetadataDefinitionResource> GetDefinitions(Guid identity)
        {
            var resources = (from bag in _metaDataDefinitionGroupBagProjectionRepository.ToQueryable()
                    from def in _definitionRepository.ToQueryable()
                    where bag.AggregateIdentity == identity && bag.DefinitionId == def.Identity
                    select new MetadataDefinitionResource
                        {
                            DataType = def.DataType,
                            Identity = def.Identity,
                            Name= def.Name,
                            Description = def.Description,
                            Regex = GetRegex(def.Regex, def.DataType)
                        }).ToArray();

            //foreach (var x in resources.Select(res => new {
            //            res,
            //            values = _definitionValueRepository.ToQueryable()
            //                .Where(v => v.DefinitionId == res.Identity)
            //                .Select(v => v.Value).ToFuture()
            //        }))
            //{
            //    x.res.Values = new HashSet<string>(x.values);
            //}
            return resources;
        }

        private string GetRegex(string regex, string dataType)
        {
            if (string.IsNullOrEmpty(regex))
            {
                var dt = DataTypeBuilder.Create(dataType) as RegexDataType;
                if (dt == null)
                    return null;
                return dt.Regex;
            }

            return regex;
        }

        private readonly IReadStoreRepository<MetadataDefinitionGroupProjection> _metaDataDefinitionGroupProjectionRepository;
        private readonly IReadStoreRepository<MetadataDefinitionGroupBagProjection> _metaDataDefinitionGroupBagProjectionRepository;
        private readonly IReadStoreRepository<MetadataDefinitionProjection> _definitionRepository;
        private readonly IReadStoreRepository<MetadataDefinitionValueProjection> _definitionValueRepository;
    }
}