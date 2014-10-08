using System;
using Lacjam.Core.Domain.MetadataDefinitions;

namespace Lacjam.Core.Services
{
    public interface IMetadataDefinitionIndexer
    {
        void SaveIndex(MetadataDefinitionProjection definition);
        MetadataDefinitionSearchResults Search(string q, int pageSize, int page);
        void DeleteIndex(Guid id);
    }
}