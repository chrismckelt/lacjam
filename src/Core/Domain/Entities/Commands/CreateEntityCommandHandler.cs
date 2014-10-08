using Lacjam.Framework.Handlers;

namespace Lacjam.Core.Domain.Entities.Commands
{
    public class CreateEntityCommandHandler : ICommandHandler<CreateEntityCommand>
    {
        public CreateEntityCommandHandler(IEntityCreationService entityCreationService)
        {
            _entityCreationService = entityCreationService;
        }

        public void Handle(CreateEntityCommand command)
        {
            _entityCreationService.Create(command.Identity, command.MetadataDefinitionGroupIdentity, command.Name, command.Values);
        }

        private readonly IEntityCreationService _entityCreationService;
    }
}