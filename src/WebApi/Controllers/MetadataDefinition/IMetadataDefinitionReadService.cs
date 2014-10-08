using System;
using System.Collections.Generic;
using Lacjam.Framework.FP;

namespace Lacjam.WebApi.Controllers.MetadataDefinition
{
    public interface IMetadataDefinitionReadService
    {
        IMaybe<MetadataDefinitionResource> FindByIdentity(Guid identity);
        IMaybe<MetadataDefinitionResource> FindByName(string name);
        IEnumerable<MetadataDefinitionSelectResource> SearchSelections(string q, int pageSize, int pageIndex);
    }
}