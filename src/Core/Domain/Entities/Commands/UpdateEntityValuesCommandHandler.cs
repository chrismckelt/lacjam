using Lacjam.Framework.Handlers;
using Lacjam.Framework.Model;

namespace Lacjam.Core.Domain.Entities.Commands
{
    public class UpdateEntityValuesCommandHandler : ICommandHandler<UpdateEntityValuesCommand>
    {

        public UpdateEntityValuesCommandHandler(IRepository<Entity> repository)
        {
            _repository = repository;
        }

        public void Handle(UpdateEntityValuesCommand command)
        {
            var aggregrate = _repository.Get(command.AggregateIdentity);
            aggregrate.Foreach(x => x.SynchronizeValues(command.DefinitionValues));

            _repository.Save(aggregrate);
        }

        private readonly IRepository<Entity> _repository;
    }
}