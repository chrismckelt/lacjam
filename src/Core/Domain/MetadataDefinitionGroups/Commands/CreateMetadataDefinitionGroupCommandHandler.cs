using Lacjam.Framework.Handlers;

namespace Lacjam.Core.Domain.MetadataDefinitionGroups.Commands
{

    public class CreateMetadataDefinitionGroupCommandHandler : ICommandHandler<CreateMetadataDefinitionGroupCommand>
    {
    
        public CreateMetadataDefinitionGroupCommandHandler(IMetadataDefinitionGroupService metadataDefinitionGroupService)
        {
            _metadataDefinitionGroupService = metadataDefinitionGroupService;
        }

        public void Handle(CreateMetadataDefinitionGroupCommand command)
        {
            _metadataDefinitionGroupService.Create(command.Identity, command.Name, command.Description, command.Bag);
        }

        private readonly IMetadataDefinitionGroupService _metadataDefinitionGroupService;
    }
}
