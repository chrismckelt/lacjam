using Lacjam.Framework.Extensions;
using Lacjam.Framework.Handlers;
using Lacjam.Framework.Model;

namespace Lacjam.Core.Domain.MetadataDefinitionGroups.Commands
{
    public class AssociateDefinitionsToMetadataDefinitionGroupCommandHandler : ICommandHandler<AssociateDefinitionsToMetadataDefinitionGroupCommand>
    {

        public AssociateDefinitionsToMetadataDefinitionGroupCommandHandler(IRepository<MetadataDefinitionGroup> repository)
        {
            _repository = repository;
        }

        public void Handle(AssociateDefinitionsToMetadataDefinitionGroupCommand command)
        {
            var entity = _repository.Get(command.Identity);

            entity.Foreach(x =>
            {
                command.DefinitionIds.Each(x.AssociateAttribute);
                x.RemoveNonExistentDefinitions(command.DefinitionIds);
            });

            _repository.Save(entity);
        }

        private readonly IRepository<MetadataDefinitionGroup> _repository;
    }
}