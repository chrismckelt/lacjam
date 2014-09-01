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
                x.ClearAttributes();
                command.DefinitionIds.Each(x.AssociateAttribute);
            });

            _repository.Save(entity);
        }

        private readonly IRepository<MetadataDefinitionGroup> _repository;
    }
}