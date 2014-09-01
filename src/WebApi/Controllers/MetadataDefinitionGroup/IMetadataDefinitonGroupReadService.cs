using System;
using Lacjam.Framework.FP;

namespace Lacjam.WebApi.Controllers.MetadataDefinitionGroup
{
    public interface IMetadataDefinitonGroupReadService
    {
        IMaybe<MetadataDefinitionGroupResource> FindByIdentity(Guid identity);
    }
}