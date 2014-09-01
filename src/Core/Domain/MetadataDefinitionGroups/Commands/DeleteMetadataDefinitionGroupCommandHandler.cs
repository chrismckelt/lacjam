using Lacjam.Framework.Handlers;
using Lacjam.Framework.Model;

namespace Lacjam.Core.Domain.MetadataDefinitionGroups.Commands
{
    public class DeleteMetadataDefinitionGroupCommandHandler : ICommandHandler<DeleteMetadataDefinitionGroupCommand>
    {

        public DeleteMetadataDefinitionGroupCommandHandler(IRepository<MetadataDefinitionGroup> repository )
        {
            _repository = repository;
        }

        public void Handle(DeleteMetadataDefinitionGroupCommand command)
        {
            var metadatagroup = _repository.Get(command.Identity);
            metadatagroup.Foreach(x => x.Delete());
            _repository.Save(metadatagroup);
        }

        private readonly IRepository<MetadataDefinitionGroup> _repository;
    }
}