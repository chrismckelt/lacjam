using System;
using Structerre.MetaStore.Framework.FP;
using Structerre.MetaStore.Framework.Model;

namespace Structerre.MetaStore.Core.Domain.Entity
{
    public class CreateEntityService : ICreateEntityService
    {
        public CreateEntityService(IRepository<Entity> repository)
        {
            _repository = repository;
        }

        public void CreateEntity(Guid conceptIdentity, string name, string description)
        {
            var concept = new Entity(conceptIdentity, new EntityName(name), new EntityDescription(description));
            _repository.Save(concept.ToMaybe());
        }

        private readonly IRepository<Entity> _repository;
    }
}
