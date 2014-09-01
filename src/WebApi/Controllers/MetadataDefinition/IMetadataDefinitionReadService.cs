using System;
using Lacjam.Framework.FP;

namespace Lacjam.WebApi.Controllers.MetadataDefinition
{
    public interface IMetadataDefinitionReadService
    {
        IMaybe<MetadataDefinitionResource> FindByIdentity(Guid identity);
    }
}