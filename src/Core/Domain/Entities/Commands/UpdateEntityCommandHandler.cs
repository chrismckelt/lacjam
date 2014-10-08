using Lacjam.Framework.Handlers;
using Lacjam.Framework.Model;

namespace Lacjam.Core.Domain.Entities.Commands
{
    public class UpdateEntityCommandHandler : 
        ICommandHandler<ReLabelEntityCommand>,
        ICommandHandler<EntityChangedGroupCommand>
    {

        public UpdateEntityCommandHandler(IRepository<Entity> repository)
        {
            _repository = repository;
        }

        public void Handle(ReLabelEntityCommand command)
        {
            var aggregate = _repository.Get(command.AggregateIdentity);
            aggregate.Foreach(x => x.ChangeEntityName(command.Name));
            _repository.Save(aggregate);
        }

        private readonly IRepository<Entity> _repository;
        public void Handle(EntityChangedGroupCommand command)
        {
            var aggregate = _repository.Get(command.AggregateIdentity);
            aggregate.Foreach(x => x.ChangeGroup(command.DefinitionGroupId));
            _repository.Save(aggregate);
        }
    }
}