using System;
using System.Collections.Generic;
using Lacjam.Core.Domain.MetadataDefinitions;
using Lacjam.Framework.FP;
using Lacjam.WebApi.Controllers.MetadataDefinition;

namespace Lacjam.WebApi.Controllers.MetadataDefinitionGroup
{
    public interface IMetadataDefinitonGroupReadService
    {
        IMaybe<MetadataDefinitionGroupResource> FindByIdentity(Guid identity);
        IMaybe<MetadataDefinitionGroupResource> FindByName(string name);
        IEnumerable<MetadataDefinitionGroupResource> GetAll();
        IEnumerable<MetadataDefinitionGroupSelectResource> SearchSelections(string q, int pageSize, int page);
        IEnumerable<MetadataDefinitionResource> GetDefinitions(Guid identity);
    }
}