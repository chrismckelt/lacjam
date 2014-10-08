using System;
using System.Collections.Generic;
using Lacjam.Core.Domain.Entities.Commands;

namespace Lacjam.Core.Domain.Entities
{
    public interface IEntityCreationService
    {
        void Create(Guid identity, Guid metadataDefinitionGroupIdentity, string name, IEnumerable<ValueSet> values);
    }
}