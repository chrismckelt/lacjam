using Lacjam.Framework.Handlers;
using Lacjam.Framework.Model;

namespace Lacjam.Core.Domain.MetadataDefinitionGroups.Commands
{
    public class ReLabelMetadataDefinitionGroupCommandHandler : ICommandHandler<ReLabelMetadataDefinitionGroupCommand>
    {
        public ReLabelMetadataDefinitionGroupCommandHandler(IRepository<MetadataDefinitionGroup> repository)
        {
            _repository = repository;
        }

        public void Handle(ReLabelMetadataDefinitionGroupCommand command)
        {
            var entity = _repository.Get(command.Identity);
            
            entity.Foreach(x =>
            {
                x.ChangeName(command.Name);
                x.ChangeDescription(command.Description);
            });

            _repository.Save(entity);
        }

        private readonly IRepository<MetadataDefinitionGroup> _repository;

    }
}