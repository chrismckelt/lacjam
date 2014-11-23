using System;
using System.Collections.Generic;
using Lacjam.Core.Domain.Entities;
using Lacjam.Core.Domain.MetadataDefinitionGroups;

namespace Lacjam.Core.Services
{
    public interface IEntityIndexer
    {
        void SaveIndex(EntityProjection entity, MetadataDefinitionGroupProjection @group, IEnumerable<EntityValueProjection> values);
        EntitySearchResults SearchKeywords(string q, int pageSize, int page);
        EntitySearchResults SearchAllMetadata(string s, int pageSize, int page);
        void RenameGroup(MetadataDefinitionGroupProjection @group);
        void DeleteIndex(Guid id);
        IEnumerable<EntitySearchResults.Hit> GetByNames(string[] names);
    }
}