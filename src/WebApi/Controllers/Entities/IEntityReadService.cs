using System;
using Lacjam.Framework.FP;

namespace Lacjam.WebApi.Controllers.Entities
{
    public interface IEntityReadService
    {
        IMaybe<EntityResource> FindByIdentity(Guid identity);
        IMaybe<EntityResource> FindByName(string name);
    }
}