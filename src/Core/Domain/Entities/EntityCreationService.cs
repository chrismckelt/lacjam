using Lacjam.Core.Domain.Entities.Commands;
using Lacjam.Core.Domain.MetadataDefinitions;
using Lacjam.Framework.Extensions;
using Lacjam.Framework.FP;
using Lacjam.Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lacjam.Core.Domain.Entities
{
    public class EntityCreationService : IEntityCreationService
    {
        public EntityCreationService(IRepository<Entity> repository)
        {
            _repository = repository;
        }

        public void Create(Guid identity, Guid metadataDefinitionGroupIdentity, string name, IEnumerable<ValueSet> values)
        {
            var aggregate = new Entity(identity,metadataDefinitionGroupIdentity, new EntityName(name));

            if(values != null && values.Any())
                values.Each(x => aggregate.AddMetadataDefinitionValue(x.MetadataDefinitionIdentity, new MetadataDefinitionName(x.Name), DataTypeBuilder.Create(x.DataType), x.Regex, GetValue(x.Values)));

            _repository.Save(aggregate.ToMaybe());
        }

        private IValue GetValue(List<string> values)
        {
            if (values == null || !values.Any())
                throw new ArgumentNullException("values");

            if(values.Count == 1)
                return new SingleValue(values.First());

            return new MultiValue(values);
        }

        private readonly IRepository<Entity> _repository;
    }
}